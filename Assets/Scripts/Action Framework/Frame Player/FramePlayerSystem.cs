using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    public class FramePlayerSystem : SystemBase
    {
        private int currFrames;
        private int prevFrame;

        private int maxFrames;

        EndSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            currFrames = 0;
            prevFrame = 0;
            maxFrames = 1000;

            // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
            CommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer().ToConcurrent();

            if (currFrames > maxFrames)
            {
                currFrames = 0;
                prevFrame = 0;
            }

            currFrames++;
            int increment = currFrames - prevFrame;
            prevFrame++;

            Entities.ForEach((ref PlayAction play, in FrameData frame) =>
            {
                play.currentFrame += increment;

                if (frame.loop)
                {
                    if (play.currentFrame > frame.totalFrames)
                        play.currentFrame = 0;
                }

                play.currentFrame = math.clamp(play.currentFrame, 0, frame.totalFrames);

            }).ScheduleParallel();

            Entities.ForEach((Entity e, int entityInQueryIndex, in PlayAction play, in FrameData frame) =>
            {
                if (!frame.loop && play.currentFrame >= frame.totalFrames)
                {
                    cmd.RemoveComponent<PlayAction>(entityInQueryIndex, e);
                }

            }).ScheduleParallel();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}