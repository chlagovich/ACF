using System;
using Unity.Entities;
using UnityEngine;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InputEventSystem))]
    public class ActionSimpleSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();

            var buffer = GetBufferFromEntity<PlayingState>(true);

            Entities.WithAll<ActionSimple>().ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, in InputEvent input, in ChannelData channel) =>
            {
                if (input.triggered)
                {
                    bool exist = false;
                    if (buffer.Exists(input.owner))
                    {
                        var states = buffer[input.owner];
                        
                        for (int i = 0; i < states.Length; i++)
                        {
                            if (states[i].channel == channel.channel)
                            {
                                exist = true;
                                break;
                            }
                        }
                    }

                    if (exist)
                        return;
                    
                    var ac = cmd.Instantiate(actions[0].action);
                    cmd.AddComponent(ac, new PlayAction());
                    cmd.AddComponent(ac, new ChannelData() { channel = channel.channel });
                    cmd.AddComponent(ac, new ActionData()
                    {
                        owner = input.owner,
                        inputEvent = e
                    });
                }

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}