using Unity.Entities;

public struct PlayAction : IComponentData
{
    public int currentFrame;
    public float normlizedTime;
}
