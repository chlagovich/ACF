using System;
using Unity.Entities;

namespace SquareBattle
{
    public struct ActionCharge : IComponentData
    {
        public Entity prevAction;
        public int minChargeDuration;
        public int lastFrameNbr;
    }
}