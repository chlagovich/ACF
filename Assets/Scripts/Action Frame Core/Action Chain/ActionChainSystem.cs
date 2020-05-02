using Unity.Entities;
using ActionFrameCore;

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
            //var cmd = CommandBuffer.CreateCommandBuffer();
//
            //var frameCount = UnityEngine.Time.frameCount;
            //BufferFromEntity<ChannelMixerState> lookupMixer = GetBufferFromEntity<ChannelMixerState>();
            //Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionChain chain, in InputEvent input, in ChannelData channel) =>
            //{
            //    if (chain.index >= actions.Length)
            //        chain.index = 0;
//
            //    var playing = lookupMixer[input.owner];
            //    var nbrf = playing[(int)channel.channel].lastNbrFrames;
            //    var duration = frameCount - nbrf;
//
            //    if (prevActionData.inputEvent != e)
            //        chain.index = 0;
//
            //    if (duration > (prevFrameData.totalFrames + chain.afterFrame))
            //        chain.index = 0;
//
            //    {
            //        var currActionData = GetComponent<ActionData>(playing.currAction);
            //        if (currActionData.inputEvent != e)
            //            chain.index = 0;
            //    }
            //}).Run();
//
            //Entities.ForEach((Entity e, DynamicBuffer<ActionBufferData> actions, ref ActionChain chain, in InputEvent input, in ChannelData channel) =>
            //{
            //    if (chain.index >= actions.Length)
            //        chain.index = 0;
//
            //    var playing = GetComponent<PlayingState>(input.owner);
            //    if (playing.currAction == Entity.Null)
            //    {
            //        if (playing.prevAction != Entity.Null)
            //        {
            //            var prevActionData = GetComponent<ActionData>(playing.prevAction);
            //            var prevFrameData = GetComponent<FrameData>(playing.prevAction);
            //            var duration = frameCount - prevActionData.spawnedFrameCount;
//
            //            if (prevActionData.inputEvent != e)
            //                chain.index = 0;
//
            //            if (duration > (prevFrameData.totalFrames + chain.afterFrame))
            //                chain.index = 0;
            //        }
            //    }
            //    else
            //    {
            //        var currActionData = GetComponent<ActionData>(playing.currAction);
            //        if (currActionData.inputEvent != e)
            //            chain.index = 0;
            //    }
            //    if (input.triggered)
            //    {
//
            //        // Player.RequestPlay(cmd, input.owner, e, actions[chain.index].action, channel.channel);
            //    }
            //}).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}