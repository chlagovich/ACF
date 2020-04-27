using Unity.Entities;

public struct PlayingState : IComponentData 
{
    public Entity prevAction;
    public Entity currAction;
}
