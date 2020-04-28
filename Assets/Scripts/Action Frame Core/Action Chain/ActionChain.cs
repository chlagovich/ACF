using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionChain : IComponentData
    {
        public int beforeFrame;
        public int afterFrame;
        public int index;
        public int actionLayer;
    }
}