using Unity.Entities;

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

            var buffer = GetBufferFromEntity<PlayingState>(true);
            var channels = GetBufferFromEntity<ChannelsBuffer>(true);

            Entities.WithAll<ActionDirect>().ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, in InputEvent input, in ChannelData channel) =>
            {
                bool exist = false;
                if (buffer.Exists(input.owner))
                {
                    var states = buffer[input.owner];

                    for (int i = 0; i < states.Length; i++)
                    {
                        if (states[i].channel == channel.channel)
                        {
                            exist = true;
                            break;
                        }
                    }
                }

                bool isBlocked = false;
                if (channels.Exists(input.owner))
                {
                    var ch = channels[input.owner];

                    for (int i = 0; i < ch.Length; i++)
                    {
                        if (ch[i].channel == channel.channel && ch[i].blocked)
                        {
                            isBlocked = true;
                            break;
                        }
                    }
                }

                if (exist || isBlocked)
                    return;

                var ac = cmd.Instantiate(actions[0].action);
                cmd.AddComponent(ac, new OnPlayUpdate() { loop = true });
                cmd.AddComponent(ac, new ChannelData() { channel = channel.channel, type = channel.type });
                cmd.AddComponent(ac, new ActionData()
                {
                    owner = input.owner,
                    inputEvent = e
                });

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}