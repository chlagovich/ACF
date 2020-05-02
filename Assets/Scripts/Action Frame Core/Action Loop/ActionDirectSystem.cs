using Unity.Entities;
using ActionFrameCore;
using Unity.Mathematics;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InputEventSystem))]
    public class ActionDirectSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();
            BufferFromEntity<ChannelEntry> lookupEntry = GetBufferFromEntity<ChannelEntry>();

            Entities.ForEach((Entity e, ref ActionDirect action, in InputEvent input, in ChannelData channel) =>
            {        
                int ch = (int)channel.channel;
                var buffer = lookupEntry[input.owner];
                buffer.RemoveAt(ch);
                buffer.Insert(ch, new ChannelEntry()
                {
                    action = action.action,
                    channel = channel.channel,
                    source = e
                });

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}