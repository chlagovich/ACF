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
            Entities.ForEach((Entity entity, ref InputEvent inputData, in InputEventOwner owner) =>
            {
                inputData.triggered = false;
                inputData.value = 0;
                var p = EntityManager.GetComponentObject<PlayerInput>(owner.owner);
                var action = p.actions.FindAction(inputData.id);
                if (action.triggered)
                {
                    if (!owners.Contains(owner.owner))
                    {
                        owners.Add(owner.owner);
                        priorities.Add(inputData.priority);
                        inputs.Add(entity);
                    }
                    else
                    {
                        int index = owners.IndexOf(owner.owner);
                        if (priorities[index] < inputData.priority)
                        {
                            priorities[index] = inputData.priority;
                            inputs[index] = entity;
                        }
                    }
                }
            }).WithoutBurst().Run();


            for (int i = 0; i < inputs.Length; i++)
            {
                var input = GetComponent<InputEvent>(inputs[i]);
                var owner = GetComponent<InputEventOwner>(inputs[i]);
                var p = EntityManager.GetComponentObject<PlayerInput>(owner.owner);
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