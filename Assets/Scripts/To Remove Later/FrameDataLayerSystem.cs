using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

// TODO : review system performance multithread
namespace SquareBattle
{
    //[UpdateInGroup(typeof(FrameDataGroupSimulation))]
    //[UpdateAfter(typeof(FrameCoreSystem))]
    //public class FrameDataLayerSystem : SystemBase
    //{
    //    EndSimulationEntityCommandBufferSystem CommandBuffer;
    //    EntityQuery NonPlayingQuery;
    //    EntityQuery PlayingQuery;
    //
    //    protected override void OnCreate()
    //    {
    //        CommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    //        NonPlayingQuery = GetEntityQuery(ComponentType.ReadOnly(typeof(FrameData)),
    //            ComponentType.ReadOnly(typeof(TargetEntiyData)),
    //            ComponentType.ReadOnly(typeof(QueueFrame)),
    //            ComponentType.Exclude<PlayFrame>());
    //
    //        PlayingQuery = GetEntityQuery(ComponentType.ReadOnly(typeof(FrameData)),
    //           ComponentType.ReadOnly(typeof(TargetEntiyData)),
    //           ComponentType.ReadOnly(typeof(PlayFrame)),
    //           ComponentType.Exclude(typeof(QueueFrame)));
    //    }
    //
    //    protected override void OnUpdate()
    //    {
    //        var cmd = CommandBuffer.CreateCommandBuffer();
    //
    //        NonPlayingQuery.CalculateEntityCount();
    //        PlayingQuery.CalculateEntityCount();
    //
    //        NativeArray<Entity> nonPlayingEntities = NonPlayingQuery.ToEntityArray(Allocator.TempJob);
    //        NativeArray<TargetEntiyData> nonPlayingTargets = NonPlayingQuery.ToComponentDataArray<TargetEntiyData>(Allocator.TempJob);
    //        NativeArray<TargetEntiyData> playingTargets = PlayingQuery.ToComponentDataArray<TargetEntiyData>(Allocator.TempJob);
    //        NativeList<Entity> targets = new NativeList<Entity>(Allocator.TempJob);
    //
    //        for (int j = 0; j < nonPlayingTargets.Length; j++)
    //        {
    //            if (!targets.Contains(nonPlayingTargets[j].target))
    //            {
    //                targets.Add(nonPlayingTargets[j].target);
    //            }
    //        }
    //
    //        for (int i = 0; i < targets.Length; i++)
    //        {
    //            // TODO layer system
    //            if (IsPlayingInSameEntity(targets[i], ref playingTargets))
    //                continue;
    //
    //            int minIndex = 0;
    //            double max = nonPlayingLifeTimes[0].startTime;
    //            for (int k = 0; k < nonPlayingEntities.Length; k++)
    //            {
    //                if (targets[i] == nonPlayingTargets[k].target && nonPlayingLifeTimes[k].startTime > max)
    //                {
    //                    max = nonPlayingLifeTimes[k].startTime;
    //                    minIndex = k;
    //                }
    //            }
    //
    //            cmd.AddComponent(nonPlayingEntities[minIndex], new PlayFrame()
    //            {
    //                currentFrame = 0,
    //                startedTime = Time.ElapsedTime
    //            });
    //        }
    //
    //        nonPlayingEntities.Dispose();
    //        targets.Dispose();
    //        nonPlayingTargets.Dispose();
    //        playingTargets.Dispose();
    //        CommandBuffer.AddJobHandleForProducer(Dependency);
    //    }
    //
    //    private bool IsPlayingInSameEntity(Entity target, ref NativeArray<TargetEntiyData> playingTargets)
    //    {
    //        for (int i = 0; i < playingTargets.Length; i++)
    //        {
    //            if (target == playingTargets[i].target)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}
}