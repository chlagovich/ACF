using Unity.Entities;

namespace ActionFrameCore
{
    public static class Player
    {
        public static void RequestPlaySet(EntityCommandBuffer cmd, Entity reciever, Entity inputEvent, Entity action, int queueDuration, int priority)
        {
            cmd.SetComponent(reciever, new RequestAction()
            {
                action = action,
                priority = priority,
                queueDuration = queueDuration,
                inputEvent = inputEvent
            });
        }

        public static void RequestPlayAdd(EntityCommandBuffer cmd, Entity reciever, Entity inputEvent, Entity action, int queueDuration, int priority)
        {
            cmd.AddComponent(reciever, new RequestAction()
            {
                action = action,
                priority = priority,
                queueDuration = queueDuration,
                inputEvent = inputEvent
            });
        }
    }
}