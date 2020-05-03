using Unity.Entities;

namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    public class ChannelMixerSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
            CommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();
            var frameCount = UnityEngine.Time.frameCount;

            Entities.WithAll<ActionData>().ForEach((Entity e, in ChannelData channel) =>
            {
                // TODO check all actions with same entity and channel 
                // stop play based on channel 
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}