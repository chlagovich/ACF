using Unity.Entities;

public struct ChannelEntry : IBufferElementData
{
    public ActionChannel channel;
    public Entity action;
    public Entity source;

}
