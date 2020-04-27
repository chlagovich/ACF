using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace SquareBattle
{
    [UpdateInGroup(typeof(FrameDataGroupSimulation))]
    public class PlayingStateSystem : SystemBase
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

            Entities.WithAll<PlayAction>().ForEach((Entity e, in ActionData action) =>
            {
                if (HasComponent<PlayingState>(action.owner))
                {
                    var state = GetComponent<PlayingState>(action.owner);
                    if (state.currentAction != e)
                    {
                        state.currentAction = e;
                        cmd.SetComponent(action.owner, state);
                    }
                }
            }).Schedule();

            Entities.WithNone<PlayAction>().ForEach((Entity e, in ActionData action) =>
            {
                if (HasComponent<PlayingState>(action.owner))
                {
                    var state = GetComponent<PlayingState>(action.owner);
                    if (state.currentAction == e)
                    {
                        state.prevAction = state.currentAction;
                        state.currentAction = Entity.Null;
                        cmd.SetComponent(action.owner, state);
                    }
                }
            }).Schedule();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}