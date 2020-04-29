using Unity.Entities;
using ActionFrameCore;
using Unity.Mathematics;

namespace SquareBattle
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InputEventSystem))]
    public class ActionLoopSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem CommandBuffer;

        protected override void OnCreate()
        {
            CommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var cmd = CommandBuffer.CreateCommandBuffer();

            var frameCount = UnityEngine.Time.frameCount;

            Entities.ForEach((Entity e, ref ActionLoop action, in InputEvent input) =>
            {
                if (HasComponent<RequestAction>(input.owner))
                    Player.RequestPlaySet(cmd, input.owner, e, action.action, 1, action.actionLayer);
                else
                    Player.RequestPlayAdd(cmd, input.owner, e, action.action, 1, action.actionLayer);

            }).Run();

            CommandBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}