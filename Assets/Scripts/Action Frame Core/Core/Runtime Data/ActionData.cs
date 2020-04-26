using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct ActionData : IComponentData
{
    public byte id;
    public byte layer;
    public Entity inputHandler;
    public Entity owner;
}
