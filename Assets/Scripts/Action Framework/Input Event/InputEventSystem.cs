using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

//TODO optimize input filtering
namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class InputEventSystem : SystemBase
    {
        protected override void OnUpdate()
        {

            var finalgroup = new NativeList<Entity>(Allocator.TempJob);
            NativeList<Entity> activeInputs = new NativeList<Entity>(Allocator.TempJob);

            Entities.ForEach((Entity entity, ref InputEvent input, in ChannelData channel) =>
            {
                input.value = 0;
                input.axis = float2.zero;
                if (input.inputResetDuration > 0)
                {
                    if (input.lastInputFrame <= FramePlayerSystem.currentFrame)
                        input.triggered = false;
                }
                else
                    input.triggered = false;

                var p = EntityManager.GetComponentObject<PlayerInput>(input.owner);
                var action = p.actions.FindAction(input.id);
                if (action.triggered)
                    input.lastInputFrame = FramePlayerSystem.currentFrame + input.inputResetDuration;

                object value = action.ReadValueAsObject();
                if (value != null)
                    activeInputs.Add(entity);

            }).WithoutBurst().Run();

            Job.WithCode(() =>
            {
                var ownergroup = new NativeList<Entity>(Allocator.TempJob);
                for (int i = 0; i < activeInputs.Length; i++)
                {
                    var input = GetComponent<InputEvent>(activeInputs[i]);
                    if (!ownergroup.Contains(input.owner))
                        ownergroup.Add(input.owner);
                }


                for (int i = 0; i < ownergroup.Length; i++)
                {
                    var group = new NativeList<Entity>(Allocator.TempJob);

                    for (int j = 0; j < activeInputs.Length; j++)
                    {
                        var input = GetComponent<InputEvent>(activeInputs[j]);
                        if (ownergroup[i] == input.owner)
                            group.Add(activeInputs[j]);

                    }

                    var channels = new NativeList<int>(Allocator.TempJob);
                    for (int k = 0; k < group.Length; k++)
                    {
                        var channel = GetComponent<ChannelData>(group[k]);
                        if (!channels.Contains((int)channel.channel))
                            channels.Add((int)channel.channel);
                    }

                    for (int l = 0; l < channels.Length; l++)
                    {
                        var chgroup = new NativeList<Entity>(Allocator.TempJob);
                        for (int s = 0; s < group.Length; s++)
                        {
                            var channel = GetComponent<ChannelData>(group[s]);
                            if (channels[l] == (int)channel.channel)
                                chgroup.Add(group[s]);
                        }

                        int index = 0;
                        for (int t = 0; t < chgroup.Length; t++)
                        {
                            var tt = GetComponent<InputEvent>(chgroup[t]);
                            var tti = GetComponent<InputEvent>(chgroup[index]);
                            if (tt.priority > tti.priority)
                                index = t;
                        }
                        finalgroup.Add(chgroup[index]);
                        chgroup.Dispose();
                    }

                    channels.Dispose();
                    group.Dispose();
                }

                ownergroup.Dispose();

            }).Run();

            activeInputs.Dispose();

            for (int i = 0; i < finalgroup.Length; i++)
            {
                var input = GetComponent<InputEvent>(finalgroup[i]);
                var p = EntityManager.GetComponentObject<PlayerInput>(input.owner);
                var action = p.actions.FindAction(input.id);
                if (action.triggered)
                    input.triggered = action.triggered;

                object value = action.ReadValueAsObject();
                if (value != null)
                {
                    var t = value.GetType();
                    if (t.Equals(typeof(Vector2)))
                        input.axis = (Vector2)value;
                    else if (t.Equals(typeof(float)))
                        input.value = (float)value;
                }
                SetComponent(finalgroup[i], input);
            }

            finalgroup.Dispose();
        }
    }
}