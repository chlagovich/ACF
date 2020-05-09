using Unity.Entities;

namespace SquareBattle
{
    public struct PlayingState : IBufferElementData
    {
        public Channel channel;
        public ChannelType channelType;
        public Entity action;
    }
}