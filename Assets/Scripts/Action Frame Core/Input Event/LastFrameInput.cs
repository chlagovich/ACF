using Unity.Entities;

public struct LastInputEvent : IComponentData
{
    public int lastFrameCount;
    public Entity lastInputEvent;
}