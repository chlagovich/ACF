using System.Collections;
using Unity.Entities;

public enum Channel { None = 0, gravity = 1, mouvement = 2, ability = 3, hit = 4 }
public enum ChannelType { Override, Additive }

public struct ChannelsBuffer : IBufferElementData
{
    public bool blocked;
    public Channel channel;
}

public struct ChannelData : IComponentData
{
    public Channel channel;
    public ChannelType type;
}
