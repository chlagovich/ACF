using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Collections;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ActionInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref ActionInput input, in ActionData data) =>
            {
                var inputEvent = GetComponent<InputEvent>(data.inputEvent);
                input.value = inputEvent.value;
                input.axis = inputEvent.axis;
                input.triggered = inputEvent.triggered;
            }).Run();

        }
    }
}