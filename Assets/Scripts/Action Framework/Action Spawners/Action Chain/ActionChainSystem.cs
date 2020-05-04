using System;
using Unity.Entities;

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

            var buffer = GetBufferFromEntity<PlayingState>(true);
            var frameCount = UnityEngine.Time.frameCount;
            Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionChain chain, in InputEvent input, in ChannelData channel) =>
            {
                if (input.triggered)
                {
                    bool exist = false;
                    int totalFrames = 0;
                    if (buffer.Exists(input.owner))
                    {
                        var states = buffer[input.owner];

                        for (int i = 0; i < states.Length; i++)
                        {
                            var data = GetComponent<ActionData>(states[i].action);
                            var frame = GetComponent<FrameData>(states[i].action);
                            if (Guid.Equals(data.id, chain.lastAction))
                            {
                                exist = true;
                                totalFrames = frame.totalFrames;
                                break;
                            }
                        }
                    }

                    if (exist)
                        return;

                    if (chain.index >= actions.Length)
                        chain.index = 0;

                    var duration = frameCount - chain.lastFrameNbr;
                    if (duration > (totalFrames + chain.resetChainDuration))
                        chain.index = 0;

                    // TODO detect input change must be implemented in the owner last input pressed

                    var id = Guid.NewGuid();
                    chain.lastAction = id;
                    var ac = cmd.Instantiate(actions[chain.index].action);
                    chain.lastFrameNbr = frameCount;
                    cmd.AddComponent(ac, new PlayAction());
                    cmd.AddComponent(ac, new ChannelData() { channel = channel.channel });
                    cmd.AddComponent(ac, new ActionData()
                    {
                        owner = input.owner,
                        inputEvent = ac,
                        id = id
                    });

                    chain.index++;
                }
            }).WithoutBurst().Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}