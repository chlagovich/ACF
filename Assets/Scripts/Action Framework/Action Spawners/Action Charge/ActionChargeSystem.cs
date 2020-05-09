using Unity.Entities;
using UnityEngine;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InputEventSystem))]
    public class ActionChargeSystem : SystemBase
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
            var frameCount = FramePlayerSystem.currentFrame;
            Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionCharge charge, in InputEvent input, in ChannelData channel) =>
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
                            charge.prevAction = states[i].action;
                            break;
                        }
                    }
                }

                if (input.value > 0)
                {
                    if (exist)
                        return;

                    charge.lastFrameNbr = frameCount + charge.minChargeDuration;
                    var ac = cmd.Instantiate(actions[0].action);
                    cmd.AddComponent(ac, new OnPlayUpdate());
                    cmd.AddComponent(ac, new ChannelData() { channel = channel.channel, type = channel.type });
                    cmd.AddComponent(ac, new ActionData()
                    {
                        owner = input.owner,
                        inputEvent = e
                    });
                }
                else
                {
                    if(frameCount > charge.lastFrameNbr)
                    {

                    }
                }

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}