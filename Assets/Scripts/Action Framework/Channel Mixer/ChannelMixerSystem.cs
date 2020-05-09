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
        //EntityQuery playing;
        protected override void OnCreate()
        {
            // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
            CommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            //playing = GetEntityQuery(typeof(PlayingState));
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();
            var frameCount = UnityEngine.Time.frameCount;

            //int length = playing.CalculateEntityCount();

            //var p = playing.ToEntityArray(Allocator.TempJob);
            //var buffer = GetBufferFromEntity<PlayingState>();
            //Entities.ForEach((Entity e, in PlayAction action) =>
            //{
            //    EntityManager.SetEnabled(e, false);
//
            //}).WithoutBurst().Run();
            /*for (int i = 0; i < p.Length; i++)
            {
                var b = buffer[p[i]];
                if (GetPlayingStateIndexByChannel(Channel.Debug, ref b))
                {
                    for (int j = 0; j < b.Length; i++)
                    {
                        if ((int)b[j].channel < (int)Channel.Debug)
                            EntityManager.SetEnabled(b[j].action, false);
                        else
                            EntityManager.SetEnabled(b[j].action, true);
                    }
                    p.Dispose();
                    return;
                }

                if (GetPlayingStateIndexByChannel(Channel.AbilityOverride, ref b))
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if ((int)b[j].channel < (int)Channel.AbilityOverride)
                            EntityManager.SetEnabled(b[j].action, false);
                        else
                            EntityManager.SetEnabled(b[j].action, true);
                    }

                    p.Dispose();
                    return;
                }

                if (GetPlayingStateIndexByChannel(Channel.Ability, ref b))
                {
                    for (int j = 0; j < b.Length; j++)
                    {
                        if ((int)b[j].channel < (int)Channel.Ability)
                            EntityManager.SetEnabled(b[j].action, false);
                        else
                            EntityManager.SetEnabled(b[j].action, true);
                    }

                    p.Dispose();
                    return;
                }

                for (int j = 0; j < b.Length; j++)
                {
                    EntityManager.SetEnabled(b[j].action, true);
                }
            }*/

            // p.Dispose();

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