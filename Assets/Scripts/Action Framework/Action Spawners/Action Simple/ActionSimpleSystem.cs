﻿using System;
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

            var playing = GetBufferFromEntity<PlayingState>(true);
            var channels = GetBufferFromEntity<ChannelsBuffer>(true);
            Entities.WithAll<ActionSimple>().ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, in InputEvent input, in ChannelData channel) =>
            {
                if (input.triggered)
                {
                    bool exist = false;
                    if (playing.Exists(input.owner))
                    {
                        var states = playing[input.owner];

                        for (int i = 0; i < states.Length; i++)
                        {
                            if (states[i].channel == channel.channel)
                            {
                                exist = true;
                                break;
                            }
                        }
                    }

                    bool isBlocked = false;
                    if (channels.Exists(input.owner))
                    {
                        var ch = channels[input.owner];

                        for (int i = 0; i < ch.Length; i++)
                        {
                            if (ch[i].channel == channel.channel && ch[i].blocked)
                            {
                                isBlocked = true;
                                break;
                            }
                        }
                    }

                    // todo changed to repeated action
                    if (exist || isBlocked)
                        return;

                    var ac = cmd.Instantiate(actions[0].action);
                    cmd.AddComponent(ac, new OnPlayUpdate());
                    cmd.AddComponent(ac, new ChannelData() { channel = channel.channel, type = channel.type });
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