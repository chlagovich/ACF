﻿using Unity.Collections;
using Unity.Entities;

namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    public class PlayingStateSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();

            Entities.ForEach((Entity entity, DynamicBuffer<PlayingState> state) =>
            {
                state.Clear();
            }).Run();

            var buffer = GetBufferFromEntity<PlayingState>();

            Entities.ForEach((Entity entity, in PlayAction play, in ActionData action, in ChannelData channel) =>
            {          
                DynamicBuffer<PlayingState> states;
                if (buffer.Exists(action.owner))
                    states = buffer[action.owner];
                else
                    states = cmd.AddBuffer<PlayingState>(action.owner);

                bool exist = false;
                for (int i = 0; i < states.Length; i++)
                {
                    if (states[i].action == entity)
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    states.Add(new PlayingState()
                    {
                        action = entity,
                        channel = channel.channel
                    });
                }
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}