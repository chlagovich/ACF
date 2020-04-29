using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionLoop : IComponentData
    {
        public Entity action;
        public int actionLayer;
    }
}