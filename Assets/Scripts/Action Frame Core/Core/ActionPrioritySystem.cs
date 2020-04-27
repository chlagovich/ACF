using Unity.Entities;
using ActionFrameCore;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ActionPrioritySystem : SystemBase
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
            var frameCount = UnityEngine.Time.frameCount;

            Entities.ForEach((Entity e, in RequestAction request, in PlayingState state) =>
            {
                if (state.currAction == Entity.Null)
                {
                    cmd.RemoveComponent<RequestAction>(e);
                    var a = cmd.Instantiate(request.action);
                    cmd.AddComponent(a, new ActionData()
                    {
                        owner = e,
                        priority = request.priority,
                        inputEvent = request.inputEvent,
                        spawnedFrameCount = frameCount
                    });
                    cmd.AddComponent(a, new PlayAction() { });
                }
                else
                {
                    var actionData = GetComponent<ActionData>(state.currAction);
                    if (request.priority > actionData.priority)
                    {
                        cmd.RemoveComponent<RequestAction>(e);
                        cmd.RemoveComponent<PlayAction>(state.currAction);
                        var a = cmd.Instantiate(request.action);
                        cmd.AddComponent(a, new ActionData()
                        {
                            owner = e,
                            priority = request.priority,
                            inputEvent = request.inputEvent,
                            spawnedFrameCount = frameCount
                        });
                        cmd.AddComponent(a, new PlayAction() { });
                    }
                }

                if (frameCount > request.queueDuration)
                    cmd.RemoveComponent<RequestAction>(e);

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}