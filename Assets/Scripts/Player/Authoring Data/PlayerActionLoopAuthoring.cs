using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

namespace SquareBattle
{
    public class PlayerActionLoopAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public PlayerActionLoop[] playerLoopActions;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            for (int i = 0; i < playerLoopActions.Length; i++)
            {
                var e = dstManager.CreateEntity();
                #if UNITY_EDITOR
                dstManager.SetName(e, dstManager.GetName(entity) + " Action " + (i + 1));
                #endif

                dstManager.AddComponentData(e, new InputEvent()
                {
                    owner = entity,
                    priority = playerLoopActions[i].inputPriority,
                    id = playerLoopActions[i].input.action.id
                });

                dstManager.AddComponentData(e, new ActionLoop()
                {
                    action = conversionSystem.GetPrimaryEntity(playerLoopActions[i].action),
                    actionLayer = (int)playerLoopActions[i].actionLayer
                });
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            for (int i = 0; i < playerLoopActions.Length; i++)
            {
                referencedPrefabs.Add(playerLoopActions[i].action);
            }
        }
    }
}