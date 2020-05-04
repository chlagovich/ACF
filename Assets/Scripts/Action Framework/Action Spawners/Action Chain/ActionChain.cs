using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionChain : IComponentData
    {
        public int index;
        public Guid lastAction;
        public int lastFrameNbr;
        public int resetChainDuration;
    }
}