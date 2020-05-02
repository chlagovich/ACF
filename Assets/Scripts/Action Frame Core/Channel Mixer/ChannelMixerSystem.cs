using Unity.Entities;
using ActionFrameCore;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ChannelMixerSystem : SystemBase
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

            Entities.ForEach((Entity e, DynamicBuffer<ChannelMixerState> state, DynamicBuffer<ChannelEntry> request) =>
            {
                for (int i = 0; i < request.Length; i++)
                {
                    if (request[i].action == Entity.Null || state[i].action != Entity.Null)
                        continue;

                    if (request[i].channel == ActionChannel.Ability ||
                        request[i].channel == ActionChannel.AbilityOverride ||
                        request[i].channel == ActionChannel.Debug)
                    {
                        int j = i - 1;
                        while (j >= 0)
                        {
                            var s = state[j];
                            if (s.action != Entity.Null)
                            {
                                cmd.RemoveComponent<PlayAction>(s.action);
                            }
                            j--;
                        }
                    }

                    var a = cmd.Instantiate(request[i].action);
                    cmd.AddComponent(a, new ActionData()
                    {
                        owner = e,
                        priority = 0,
                        inputEvent = request[i].source,
                        spawnedFrameCount = frameCount
                    });
                    cmd.AddComponent(a, new PlayAction() { });
                    state.RemoveAt(i);
                    state.Insert(i, new ChannelMixerState()
                    {
                        action = a,
                        channel = request[i].channel,
                        lastNbrFrames = frameCount
                    });
                    request.RemoveAt(i);
                    request.Insert(i, new ChannelEntry()
                    {
                        action = Entity.Null,
                        channel = request[i].channel,
                        source = Entity.Null
                    });

                }
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}