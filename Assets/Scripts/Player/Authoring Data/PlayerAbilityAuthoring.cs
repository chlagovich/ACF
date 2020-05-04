using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

namespace SquareBattle
{

    public enum AbilityType
    {
        Chain, Charge, Direct, Simple
    }

    [Serializable]
    public struct PlayerAbility
    {
        public InputActionReference input;
        public Channel channel;
        public int inputPriority;
        public AbilityType type;
        public GameObject[] actions;
    }

    public class PlayerAbilityAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {

        public int resetChainAfter;
        public int inputTriggerDuration;
        public PlayerAbility[] abilities;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                var e = dstManager.CreateEntity();

                #if UNITY_EDITOR
                var name = dstManager.GetName(entity) + " " + abilities[i].input.action.name + " Input Event";
                dstManager.SetName(e, name);
                #endif

                switch (abilities[i].type)
                {
                    case AbilityType.Simple:
                        dstManager.AddComponent(e, typeof(ActionSimple));
                        break;
                    case AbilityType.Direct:
                        dstManager.AddComponent(e, typeof(ActionDirect));
                        break;
                    case AbilityType.Chain:
                        dstManager.AddComponentData(e, new ActionChain()
                        {
                            resetChainDuration = resetChainAfter
                        });
                        break;
                    case AbilityType.Charge:
                        dstManager.AddComponent(e, typeof(ActionCharge));
                        break;

                }

                dstManager.AddComponentData(e, new ChannelData() { channel = abilities[i].channel });
                dstManager.AddComponentData(e, new InputEvent()
                {
                    owner = entity,
                    priority = abilities[i].inputPriority,
                    id = abilities[i].input.action.id,
                    inputResetDuration = inputTriggerDuration
                });

                var actions = abilities[i].actions;
                DynamicBuffer<ActionBufferData> acbuffer = dstManager.AddBuffer<ActionBufferData>(e);
                for (int j = 0; j < actions.Length; j++)
                {
                    var b = new ActionBufferData()
                    {
                        action = conversionSystem.GetPrimaryEntity(actions[j])
                    };
                    acbuffer.Add(b);
                }
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                referencedPrefabs.AddRange(abilities[i].actions);
            }
        }
    }
}