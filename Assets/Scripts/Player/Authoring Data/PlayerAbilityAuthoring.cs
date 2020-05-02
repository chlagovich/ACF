using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

namespace SquareBattle
{

    public enum ActionType
    {
        Chain, Charge
    }

    [Serializable]
    public struct PlayerAbility
    {
        public bool continious;
        public InputActionReference input;
        public int inputPriority;
        public ActionType type;
        public GameObject[] actions;
    }

    public class PlayerAbilityAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public ActionChannel channel;
        public int resetChainAfter;
        public int inputTriggerDuration;
        public PlayerAbility[] playerActions;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            for (int i = 0; i < playerActions.Length; i++)
            {
                var e = dstManager.CreateEntity();
#if UNITY_EDITOR
                dstManager.SetName(e, dstManager.GetName(entity) + " " + playerActions[i].input.action.name + " Input Event");
#endif
                //dstManager.AddComponentData(e, new ActionChain()
                //{
                //    afterFrame = resetChainAfter,
                //    beforeFrame = queueActionBefore,
                //    index = 0
                //});

                dstManager.AddComponentData(e, new ChannelData() { channel = channel });
                dstManager.AddComponentData(e, new InputEvent()
                {
                    owner = entity,
                    priority = playerActions[i].inputPriority,
                    id = playerActions[i].input.action.id,
                    inputResetDuration = inputTriggerDuration
                });

                //DynamicBuffer<ActionBufferData> acbuffer = dstManager.AddBuffer<ActionBufferData>(e);
                //for (int j = 0; j < playerActions[i].actions.Length; j++)
                //{
                //    var action = playerActions[i].actions[j];
                //    var b = new ActionBufferData() { action = conversionSystem.GetPrimaryEntity(action) };
                //    acbuffer.Add(b);
                //}
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            //for (int i = 0; i < playerActions.Length; i++)
            //{
            //    referencedPrefabs.AddRange(playerActions[i].actions);
            //}
        }
    }
}