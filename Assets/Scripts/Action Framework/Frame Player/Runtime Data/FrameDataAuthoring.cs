using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct FrameData : IComponentData
{
    public int totalFrames;
}

public class FrameDataAuthoring : MonoBehaviour
{
    public int totalFrames;
}

public class FrameDataAuthoringBaker : Baker<FrameDataAuthoring>
{
    public override void Bake(FrameDataAuthoring authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.None), new FrameData
        {
            totalFrames = authoring.totalFrames
        });
    }
}
