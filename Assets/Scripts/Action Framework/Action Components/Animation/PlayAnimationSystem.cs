using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    [UpdateAfter(typeof(ChannelMixerSystem))]
    public class PlayAnimationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, in OnPlayUpdate play, in ActionData actionData, in FrameData frameData, in AnimationData animation) =>
            {
                var animator = EntityManager.GetComponentObject<Animator>(actionData.owner);

                animator.Play(animation.name.ToString(), 0, (1f / frameData.totalFrames) * play.currentFrame);

            }).WithoutBurst().Run();
        }
    }
}