using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct AnimationData : IComponentData 
{
    public FixedString32 name;
}

public class AnimationDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public string animationName;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new AnimationData() { name = animationName });
    }
}