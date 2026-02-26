using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class FrameDataGroupSimulation : ComponentSystemGroup
{
    protected override void OnCreate()
    {
        base.OnCreate();
        World.GetExistingSystemManaged<FixedStepSimulationSystemGroup>().Timestep = 0.033333f;
    }
}
