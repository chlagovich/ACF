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

            Entities.WithAll<ActionDirect>().ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, in InputEvent input, in ChannelData channel) =>
            {
                //if(input.value > 0)
                //{
                //    var a = cmd.Instantiate(actions[0].action);
                //    cmd.AddComponent(a, new ActionData()
                //    {
                //        owner = e,
                //        inputEvent = e
                //    });
                //    cmd.AddComponent(a, new PlayAction() { });
                //}
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}