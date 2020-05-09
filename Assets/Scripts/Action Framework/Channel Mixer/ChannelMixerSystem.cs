using System;
using Unity.Collections;
using Unity.Entities;

namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    [UpdateAfter(typeof(PlayingStateSystem))]
    public class ChannelMixerSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem CommandBuffer;
        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();

            Entities.ForEach((Entity e, DynamicBuffer<PlayingState> states) =>
            {
                // TODO figure out a way to stop action as they play,
                // currently they play 2 frames and then get destroyed

                if (GetPlayingStateIndexByChannel(Channel.hit, ref states))
                {
                    for (int j = 0; j < states.Length; j++)
                    {
                        if ((int)states[j].channel < (int)Channel.hit && !HasComponent<OnStop>(states[j].action))
                            cmd.AddComponent(states[j].action, new OnStop());
                    }
                    return;
                }

                if (GetPlayingStateIndexByChannel(Channel.ability, ref states))
                {
                    for (int j = 0; j < states.Length; j++)
                    {
                        if ((int)states[j].channel < (int)Channel.ability && !HasComponent<OnStop>(states[j].action))
                            cmd.AddComponent(states[j].action, new OnStop());
                    }
                    return;
                }

                for (int j = 0; j < states.Length; j++)
                {
                    if (HasComponent<OnStop>(states[j].action))
                        cmd.RemoveComponent<OnStop>(states[j].action);
                }

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }


        public static bool GetPlayingStateIndexByChannel(Channel channel, ref DynamicBuffer<PlayingState> states)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].channel == channel)
                    return true;
            }
            return false;
        }
    }
}