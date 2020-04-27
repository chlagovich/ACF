using Unity.Entities;

namespace ActionFrameCore
{
    public static class Player
    {

        /// <summary> play action once.
        /// </summary>
        public static Entity Play(EntityCommandBuffer cmd, Entity owner, Entity action)
        {
            var a = cmd.Instantiate(action);
            cmd.AddComponent(a, new ActionData() { owner = owner });
            cmd.AddComponent(a, new PlayAction() { });
            return a;
        }

        ///<summary> queue action once.
        ///</summary>
        public static void PlayQueued(EntityCommandBuffer cmd, Entity owner, Entity action)
        {
            cmd.AddComponent(owner, new QueuedAction() { queuedAction = action });
        }

        // get previous action
        // get current action
        // is playing
        // pause action
        // destroy action
    }
}