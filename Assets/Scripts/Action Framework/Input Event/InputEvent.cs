using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using System;

namespace SquareBattle
{
    public struct InputEvent : IComponentData
    {
        public Entity owner;
        public int priority;
        public Guid id;
        public bool continuous;
        public float value;
        public bool triggered;
        public float2 axis;
        public int inputResetDuration;
        public int lastInputFrame;
    }
}