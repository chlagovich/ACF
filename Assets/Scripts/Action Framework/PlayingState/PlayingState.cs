using Unity.Entities;

namespace SquareBattle
{
    public struct PlayingState : IBufferElementData
    {
        public Channel channel;
        public Entity action;
    }
}