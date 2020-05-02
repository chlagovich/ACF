using Unity.Entities;
using ActionFrameCore;

namespace SquareBattle
{/*
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

            Entities.ForEach((Entity e, ref ChannelRequest request, in PlayingState state) =>
            {
                if (state.currAction == Entity.Null)
                {
                    cmd.RemoveComponent<ChannelRequest>(e);
                    var a = cmd.Instantiate(request.action);
                    cmd.AddComponent(a, new ActionData()
                    {
                        owner = e,
                        priority = request.priority,
                        inputEvent = request.source,
                        spawnedFrameCount = frameCount
                    });
                    cmd.AddComponent(a, new PlayAction() { });
                    if (HasComponent<ActionChain>(request.source))
                    {
                        var chain = GetComponent<ActionChain>(request.source);
                        chain.index++;
                        cmd.SetComponent(request.source, chain);
                    }
                }
                else
                {
                    var actionData = GetComponent<ActionData>(state.currAction);
                    if (request.priority > actionData.priority)
                    {
                        cmd.RemoveComponent<ChannelRequest>(e);
                        cmd.RemoveComponent<PlayAction>(state.currAction);
                        var a = cmd.Instantiate(request.action);
                        cmd.AddComponent(a, new ActionData()
                        {
                            owner = e,
                            priority = request.priority,
                            inputEvent = request.source,
                            spawnedFrameCount = frameCount
                        });
                        cmd.AddComponent(a, new PlayAction() { });
                        if (HasComponent<ActionChain>(request.source))
                        {
                            var chain = GetComponent<ActionChain>(request.source);
                            chain.index++;
                            cmd.SetComponent(request.source, chain);
                        }
                    }
                }

                if (request.queueDuration <= 0)
                    cmd.RemoveComponent<ChannelRequest>(e);

                request.queueDuration--;

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }*/
}