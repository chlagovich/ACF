using Unity.Entities;

public struct ChannelMixerState : IBufferElementData
{
    public ActionChannel channel;
    public Entity action;
    public int lastNbrFrames;
}
