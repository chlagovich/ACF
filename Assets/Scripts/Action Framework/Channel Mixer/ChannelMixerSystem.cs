using System;
using Unity.Collections;
using Unity.Entities;

namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    [UpdateAfter(typeof(PlayingStateSystem))]
    public partial class ChannelMixerSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem CommandBuffer;
        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();
            var onStopLookup = GetComponentLookup<OnStop>(true);

            Channel topChanel = Channel.None;

            Entities.ForEach((Entity e, DynamicBuffer<PlayingState> states) =>
            {
                var topIndex = GetTopOverridePlayingChannelIndex(states);
                if (topIndex != -1)
                    topChanel = states[topIndex].channel;
                for (int i = 0; i < states.Length; i++)
                {
                    if ((int)states[i].channel < (int)topChanel)
                    {
                        if (!onStopLookup.HasComponent(states[i].action))
                            cmd.AddComponent(states[i].action, new OnStop() { destroy = true });
                    }
                }

            }).Run();

            Entities.ForEach((Entity e, DynamicBuffer<ChannelsBuffer> channels) =>
            {
                for (int i = 0; i < channels.Length; i++)
                {
                    var c = channels[i];
                    if ((int)channels[i].channel < (int)topChanel)
                        c.blocked = true;
                    else
                        c.blocked = false;

                    channels[i] = c;
                }

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }

        public static int GetTopOverridePlayingChannelIndex(DynamicBuffer<PlayingState> states)
        {
            int topChannel = 1;
            int index = -1;
            for (int i = 0; i < states.Length; i++)
            {
                if ((int)states[i].channel > topChannel && states[i].channelType == ChannelType.Override)
                {
                    topChannel = (int)states[i].channel;
                    index = i;
                }
            }

            return index;
        }
    }
}
