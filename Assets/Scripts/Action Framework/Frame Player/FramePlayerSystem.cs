using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    public class FramePlayerSystem : SystemBase
    {
        public static int currentFrame;
        private int prevFrame;

        private int maxFrames;

        EndSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            currentFrame = 0;
            prevFrame = 0;

            // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
            CommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer().ToConcurrent();

            currentFrame++;
            int increment = currentFrame - prevFrame;
            prevFrame++;

            Entities.WithNone<OnPause, OnStop>().ForEach((ref OnPlayUpdate play, in FrameData frame) =>
             {
                 play.currentFrame += increment;

                 //play.normlizedTime
                 // set normalized time
                 if (play.loop)
                 {
                     if (play.currentFrame > frame.totalFrames)
                         play.currentFrame = 0;
                 }

                 play.currentFrame = math.clamp(play.currentFrame, 0, frame.totalFrames);

             }).ScheduleParallel();

            Entities.WithAll<OnStop>().ForEach((Entity e, ref OnPlayUpdate play) =>
            {
                play.currentFrame = 0;
                play.normlizedTime = 0;

            }).ScheduleParallel();

            Entities.ForEach((Entity e, int entityInQueryIndex, in OnPlayUpdate play, in FrameData frame) =>
            {
                if (!play.loop && play.currentFrame >= frame.totalFrames)
                {
                    cmd.RemoveComponent<OnPlayUpdate>(entityInQueryIndex, e);
                    cmd.AddComponent<OnStop>(entityInQueryIndex, e);
                }

            }).ScheduleParallel();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}