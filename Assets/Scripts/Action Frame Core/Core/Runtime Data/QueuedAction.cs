using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct QueuedAction : IComponentData 
{
    public Entity queuedAction;
}
