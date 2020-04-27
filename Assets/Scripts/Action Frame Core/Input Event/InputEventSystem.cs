using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Collections;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class InputEventSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            NativeList<Entity> owners = new NativeList<Entity>(Allocator.TempJob);
            NativeList<int> priorities = new NativeList<int>(Allocator.TempJob);
            NativeList<Entity> inputs = new NativeList<Entity>(Allocator.TempJob);
            Entities.ForEach((Entity entity, ref InputEvent input) =>
            {
                input.triggered = false;
                input.value = 0;
                var p = EntityManager.GetComponentObject<PlayerInput>(input.owner);
                var action = p.actions.FindAction(input.id);
                if (action.triggered)
                {
                    if (!owners.Contains(input.owner))
                    {
                        owners.Add(input.owner);
                        priorities.Add(input.priority);
                        inputs.Add(entity);
                    }
                    else
                    {
                        int index = owners.IndexOf(input.owner);
                        if (priorities[index] < input.priority)
                        {
                            priorities[index] = input.priority;
                            inputs[index] = entity;
                        }
                    }
                }
            }).WithoutBurst().Run();


            for (int i = 0; i < inputs.Length; i++)
            {
                var input = GetComponent<InputEvent>(inputs[i]);
                var p = EntityManager.GetComponentObject<PlayerInput>(input.owner);
                var action = p.actions.FindAction(input.id);
                input.triggered = action.triggered;
                input.value = action.ReadValue<float>();
                SetComponent(inputs[i], input);
            }

            owners.Dispose();
            priorities.Dispose();
            inputs.Dispose();
        }
    }
}