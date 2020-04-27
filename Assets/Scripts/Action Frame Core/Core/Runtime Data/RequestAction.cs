using Unity.Entities;

public struct RequestAction : IComponentData
{
    public int queueDuration; // queue duration in frames
    public int priority; // changed to byte
    public Entity action;
    public Entity inputEvent;

}
