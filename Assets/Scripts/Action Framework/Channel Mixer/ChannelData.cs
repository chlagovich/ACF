﻿using Unity.Entities;

public struct ChannelData : IComponentData
{
    public Channel channel;
    public ChannelType type;
}
