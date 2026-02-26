using System;
using Unity.Entities;

namespace SquareBattle
{
    [UpdateAfter(typeof(PlayingStateSystem))]
    public partial class ActionHistorySystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();

            var frameCount = FramePlayerSystem.currentFrame;
            Entities.ForEach((Entity e, DynamicBuffer<PlayingState> actions) =>
            {
                
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}