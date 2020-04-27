using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct ActionData : IComponentData
{
    public int priority;
    public Entity owner;
    public Entity inputEvent;
    public int spawnedFrameCount; 
}
