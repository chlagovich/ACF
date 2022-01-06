using Unity.Entities;

public struct OnPlayUpdate : IComponentData
{
    public bool loop;
    public int currentFrame;
    public float normlizedTime;
}
