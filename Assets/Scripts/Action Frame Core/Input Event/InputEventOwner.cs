using Unity.Entities;

public struct InputEventOwner : IComponentData
{
    public Entity owner;
}