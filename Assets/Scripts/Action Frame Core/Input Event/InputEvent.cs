using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using System;

namespace SquareBattle
{
    public struct InputEvent : IComponentData
    {
        public int priority;
        public Guid id;
        public float value;
        public bool triggered;
    }
}