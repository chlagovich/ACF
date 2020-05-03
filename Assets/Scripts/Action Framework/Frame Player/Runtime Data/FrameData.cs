using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct FrameData : IComponentData
{
    public bool loop;
    public int totalFrames;
}