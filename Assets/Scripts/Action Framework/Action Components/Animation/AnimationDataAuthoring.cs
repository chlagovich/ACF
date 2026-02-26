using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct AnimationData : IComponentData 
{
    public FixedString32Bytes name;
}

public class AnimationDataAuthoring : MonoBehaviour
{
    public string animationName;

    private class Baker : Unity.Entities.Baker<AnimationDataAuthoring>
    {
        public override void Bake(AnimationDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new AnimationData() { name = authoring.animationName });
        }
    }
}
