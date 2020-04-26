using Unity.Entities;
using ActionFrameCore;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class QueueAutoPlaySystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
            CommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();
            Entities.ForEach((Entity e, in QueuedAction queue, in PlayingState state) =>
            {
                if (state.currentAction == Entity.Null)
                {
                    cmd.RemoveComponent<QueuedAction>(e);
                    var a = Player.Play(cmd, e, queue.queuedAction);
                }
            }).Schedule();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}