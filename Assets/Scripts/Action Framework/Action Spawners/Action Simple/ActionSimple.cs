using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionSimple : IComponentData
    {
        public Guid lastAction;
    }
}