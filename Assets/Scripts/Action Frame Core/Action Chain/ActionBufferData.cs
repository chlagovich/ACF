using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionBufferData : IBufferElementData
    {
        public Entity action;
    }
}