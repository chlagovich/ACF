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

            Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionChain chain, in InputEvent input, in ChannelData channel) =>
            {
                //if (chain.index >= actions.Length)
                //    chain.index = 0;
                //
                //var playing = lookupMixer[input.owner];
                //var nbrf = playing[(int)channel.channel].lastNbrFrames;
                //var duration = frameCount - nbrf;
                //
                //if (prevActionData.inputEvent != e)
                //    chain.index = 0;
                //
                //if (duration > (prevFrameData.totalFrames + chain.afterFrame))
                //    chain.index = 0;
                //
                //{
                //    var currActionData = GetComponent<ActionData>(playing.currAction);
                //    if (currActionData.inputEvent != e)
                //        chain.index = 0;
                //}
            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}