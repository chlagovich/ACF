using Unity.Entities;
using ActionFrameCore;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InputEventSystem))]
    public class ActionChainSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();

            var frameCount = UnityEngine.Time.frameCount;

            Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionChain chain, in InputEvent input) =>
            {
                if (input.triggered)
                {
                    if (chain.index >= actions.Length)
                        chain.index = 0;

                    var playing = GetComponent<PlayingState>(input.owner);
                    if (playing.currAction == Entity.Null)
                    {
                        if (playing.prevAction != Entity.Null)
                        {
                            var prevActionData = GetComponent<ActionData>(playing.prevAction);
                            var prevFrameData = GetComponent<FrameData>(playing.prevAction);
                            var duration = frameCount - prevActionData.spawnedFrameCount;

                            if (duration > (prevFrameData.totalFrames + chain.afterFrame))
                                chain.index = 0;

                            if (prevActionData.inputEvent != e)
                                chain.index = 0;
                            else
                                chain.index++;
                        }
                    }
                    else
                    {
                        var currActionData = GetComponent<ActionData>(playing.currAction);
                        if (currActionData.inputEvent != e)
                        {
                            chain.index = 0;
                        }
                    }

                    Player.RequestPlay(cmd, input.owner, e, actions[chain.index].action, frameCount + chain.beforeFrame, input.priority);

                }
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}