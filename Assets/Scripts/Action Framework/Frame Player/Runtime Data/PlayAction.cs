using Unity.Entities;

public struct PlayAction : IComponentData
{
    public bool loop;
    public int currentFrame;
    public float normlizedTime;
}
