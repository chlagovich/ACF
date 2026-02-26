using System;
using Unity.Entities;
using UnityEngine;

namespace SquareBattle
{
    [Serializable]
    public struct PlayerTag : IComponentData { }

    public class PlayerTagAuthoring : MonoBehaviour
    {
    }

    public class PlayerTagAuthoringBaker : Baker<PlayerTagAuthoring>
    {
        public override void Bake(PlayerTagAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.None), new PlayerTag());
        }
    }
}
