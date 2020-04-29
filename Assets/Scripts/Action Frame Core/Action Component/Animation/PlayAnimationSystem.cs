using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    public class PlayAnimationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<PlayAction>().ForEach((Entity e, in ActionData actionData, in FrameData frameData, in AnimationData animation) =>
            {
                var animator = EntityManager.GetComponentObject<Animator>(actionData.owner);

                animator.Play(animation.name.ToString(), 0, (1f / frameData.totalFrames) * frameData.currentFrame);

            }).WithoutBurst().Run();
        }
    }
}