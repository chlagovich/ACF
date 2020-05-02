using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionDirect : IComponentData
    {
        public Entity action;
    }
}