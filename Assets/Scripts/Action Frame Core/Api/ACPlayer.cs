using Unity.Entities;

namespace ActionFrameCore
{
    public static class Player
    {
        public static void RequestPlay(EntityCommandBuffer cmd, Entity reciever, Entity inputEvent, Entity action, int queueDuration, int priority)
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