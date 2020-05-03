using Unity.Entities;
using UnityEngine;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InputEventSystem))]
    public class ActionSimpleSystem : SystemBase
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

            Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionSimple simple, in InputEvent input, in ChannelData channel) =>
            {
                if (input.triggered)
                {
                    bool exist = false;
                    if (buffer.Exists(input.owner))
                    {
                        var states = buffer[input.owner];
                        
                        for (int i = 0; i < states.Length; i++)
                        {
                            Debug.Log(states[i].action);
                            Debug.Log(simple.lastAction);
                            if (states[i].action == simple.lastAction)
                            {
                                exist = true;
                                break;
                            }
                        }
                    }

                    if (exist)
                        return;

                    simple.lastAction = cmd.Instantiate(actions[0].action);
                    cmd.AddComponent(simple.lastAction, new PlayAction());
                    cmd.AddComponent(simple.lastAction, new ChannelData() { channel = channel.channel });
                    cmd.AddComponent(simple.lastAction, new ActionData()
                    {
                        owner = input.owner,
                        inputEvent = e
                    });
                }

            }).WithoutBurst().Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}