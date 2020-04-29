using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using System;

namespace SquareBattle
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct ActionInput : IComponentData
    {
        public float value;
        public bool triggered;
        public float2 axis;
    }
}