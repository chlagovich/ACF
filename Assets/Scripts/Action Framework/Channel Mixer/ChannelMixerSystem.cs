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
            var frameCount = UnityEngine.Time.frameCount;

           Entities.ForEach((Entity e , DynamicBuffer<PlayingState> states)=>
            {
  
                if (GetPlayingStateIndexByChannel(Channel.Debug, ref states))
                {
                    for (int j = 0; j < states.Length; j++)
                    {
                        if ((int)states[j].channel < (int)Channel.Debug)
                            cmd.AddComponent(states[j].action, new OnStop());
                    }

                    return;
                }

                if (GetPlayingStateIndexByChannel(Channel.AbilityOverride, ref states))
                {
                    for (int j = 0; j < states.Length; j++)
                    {
                        if ((int)states[j].channel < (int)Channel.AbilityOverride)
                            cmd.AddComponent(states[j].action, new OnStop());
                    }
                    return;
                }

                if (GetPlayingStateIndexByChannel(Channel.Ability, ref states))
                {
                    for (int j = 0; j < states.Length; j++)
                    {
                        if ((int)states[j].channel < (int)Channel.Ability)
                            cmd.AddComponent(states[j].action, new OnStop());
                    }
                    return;
                }

                for (int j = 0; j < states.Length; j++)
                {
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