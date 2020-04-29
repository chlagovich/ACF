using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

namespace SquareBattle
{
    public class PlayerActionChainAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public int resetChainAfter;
        public int queueActionBefore;
        public PlayerActionChain[] playerActions;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayingState()
            {
                currAction = Entity.Null,
                prevAction = Entity.Null
            });
            for (int i = 0; i < playerActions.Length; i++)
            {
                var e = dstManager.CreateEntity();
                #if UNITY_EDITOR
                dstManager.SetName(e, dstManager.GetName(entity) + " Action " + (i + 1));
                #endif
                dstManager.AddComponentData(e, new ActionChain()
                {
                    afterFrame = resetChainAfter,
                    beforeFrame = queueActionBefore,
                    index = 0,
                    actionLayer = (int)playerActions[i].actionLayer
                });

                dstManager.AddComponentData(e, new InputEvent()
                {
                    owner = entity,
                    priority = playerActions[i].inputPriority,
                    id = playerActions[i].input.action.id
                });

                DynamicBuffer<ActionBufferData> acbuffer = dstManager.AddBuffer<ActionBufferData>(e);
                for (int j = 0; j < playerActions[i].actions.Length; j++)
                {
                    var action = playerActions[i].actions[j];
                    var b = new ActionBufferData() { action = conversionSystem.GetPrimaryEntity(action) };
                    acbuffer.Add(b);
                }
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            for (int i = 0; i < playerActions.Length; i++)
            {
                referencedPrefabs.AddRange(playerActions[i].actions);
            }
        }
    }
}