using System;
using Unity.Entities;

public struct ActionData : IComponentData
{
    public Guid id;
    public Entity owner;
    public Entity inputEvent;
}
