using Unity.Entities;

namespace SquareBattle
{
    public struct ActionHistory : IBufferElementData
    {
        public int executionOrder;
        public Channel channel;
        public ChannelType channelType;
        public int totalFrames;

    }
}