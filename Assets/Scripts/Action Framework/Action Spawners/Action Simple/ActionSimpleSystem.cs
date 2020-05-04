using System;
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
                            var data = GetComponent<ActionData>(states[i].action);
                            if (Guid.Equals(data.id,simple.lastAction))
                            {
                                exist = true;
                                break;
                            }
                        }
                    }

                    if (exist)
                        return;
                    
                    var id = Guid.NewGuid();
                    
                    simple.lastAction = id;
                    var ac = cmd.Instantiate(actions[0].action);
                    cmd.AddComponent(ac, new PlayAction());
                    cmd.AddComponent(ac, new ChannelData() { channel = channel.channel });
                    cmd.AddComponent(ac, new ActionData()
                    {
                        owner = input.owner,
                        inputEvent = ac,
                        id = id
                    });
                }

            }).WithoutBurst().Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}