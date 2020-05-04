using System;
using Unity.Entities;

public struct ActionData : IComponentData
{
    public Entity owner;
    public Entity inputEvent;
}
