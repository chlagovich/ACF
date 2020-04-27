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
                    if (chain.index >= actions.Length || InputEventHasChanged(input.owner, e))
                        chain.index = 0;

                    if (HasComponent<QueuedAction>(input.owner))
                        return;

                    var playing = GetComponent<PlayingState>(input.owner);
                    var ac = playing.currentAction;
                    if (ac == Entity.Null)
                    {
                        var prevAc = playing.prevAction;
                        if (prevAc != Entity.Null)
                        {
                            var duration = frameCount - GetLastFrame(input.owner);
                            var frameData = GetComponent<FrameData>(prevAc);
                            if (duration > (frameData.totalFrames + chain.afterFrame))
                                chain.index = 0;
                        }
                        
                        Player.Play(cmd, input.owner, actions[chain.index].action);
                        chain.index++;
                        SetLastFrame(cmd, input.owner, frameCount, e);
                    }
                    else
                    {
                        var duration = frameCount - GetLastFrame(input.owner);
                        var frameData = GetComponent<FrameData>(ac);
                        if (duration > (frameData.totalFrames - chain.beforeFrame))
                        {
                            // play queued
                            Player.PlayQueued(cmd, input.owner, actions[chain.index].action);
                            chain.index++;
                            SetLastFrame(cmd, input.owner, frameCount, e);
                        }
                    }
                }

            }).WithoutBurst().Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }

        private void SetLastFrame(EntityCommandBuffer commandBuffer, Entity owner, int frame, Entity inputEvent)
        {
            if (HasComponent<LastInputEvent>(owner))
            {
                commandBuffer.SetComponent(owner, new LastInputEvent()
                {
                    lastFrameCount = frame,
                    lastInputEvent = inputEvent
                });
            }
            else
            {
                commandBuffer.AddComponent(owner, new LastInputEvent()
                {
                    lastFrameCount = frame,
                    lastInputEvent = inputEvent
                });
            }
        }

        private int GetLastFrame(Entity owner)
        {
            int lastF = 0;
            if (HasComponent<LastInputEvent>(owner))
            {
                var lfi = GetComponent<LastInputEvent>(owner);
                lastF = lfi.lastFrameCount;
            }
            return lastF;
        }
        private bool InputEventHasChanged(Entity owner, Entity inputEvent)
        {
            if (HasComponent<LastInputEvent>(owner))
            {
                var lfi = GetComponent<LastInputEvent>(owner);
                return lfi.lastInputEvent != inputEvent;
            }
            return false;
        }
    }
}