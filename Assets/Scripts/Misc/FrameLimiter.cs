using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class FrameDataGroupSimulation : ComponentSystemGroup
{
    protected override void OnCreate()
    {
        base.OnCreate();
        World.GetExistingSystem<FixedStepSimulationSystemGroup>().Timestep =  0.033333f;
    }
}