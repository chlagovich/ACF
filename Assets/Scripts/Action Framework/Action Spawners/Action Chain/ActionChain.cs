using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionChain : IComponentData
    {
        public int index;
        public Entity prevAction;
        public int lastFrameNbr;
        public int resetChainDuration;
    }
}