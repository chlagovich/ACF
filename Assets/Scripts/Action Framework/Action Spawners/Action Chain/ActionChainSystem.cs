﻿using System;
using Unity.Entities;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InputEventSystem))]
    public class ActionChainSystem : SystemBase
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
            var channels = GetBufferFromEntity<ChannelsBuffer>(true);
            var frameCount = FramePlayerSystem.currentFrame;
            Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionChain chain, in InputEvent input, in ChannelData channel) =>
            {
                bool exist = false;
                if (buffer.HasComponent(input.owner))
                {
                    var states = buffer[input.owner];

                    for (int i = 0; i < states.Length; i++)
                    {
                        if (states[i].channel == channel.channel)
                        {
                            exist = true;
                            chain.prevAction = states[i].action;
                            break;
                        }
                    }
                }

                bool isBlocked = false;
                if (channels.HasComponent(input.owner))
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

                bool trigger = false;
                if (input.continuous)
                    trigger = input.value > 0;
                else
                    trigger = input.triggered;

                if (trigger && !exist && !isBlocked)
                {
                    if (chain.index >= actions.Length)
                        chain.index = 0;

                    if (chain.prevAction != Entity.Null)
                    {
                        // todo : you need to implement new way to store previous action
                        var acData = GetComponent<ActionData>(chain.prevAction);
                        if (acData.inputEvent != e)
                            chain.index = 0;

                        var frame = GetComponent<FrameData>(chain.prevAction);
                        var duration = frameCount - chain.lastFrameNbr;
                        if (duration > (frame.totalFrames + chain.resetChainDuration))
                            chain.index = 0;
                    }

                    var ac = cmd.Instantiate(actions[chain.index].action);
                    chain.lastFrameNbr = frameCount;
                    cmd.AddComponent(ac, new OnPlayUpdate());
                    cmd.AddComponent(ac, new ChannelData() { channel = channel.channel, type = channel.type });
                    cmd.AddComponent(ac, new ActionData()
                    {
                        owner = input.owner,
                        inputEvent = e
                    });

                    chain.index++;
                }
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}