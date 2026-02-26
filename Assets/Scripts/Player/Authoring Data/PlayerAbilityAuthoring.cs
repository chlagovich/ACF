using UnityEngine;
using Unity.Entities;
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
        public bool continuous;
        public Channel channel;
        public ChannelType channelType;
        public int inputPriority;
        public AbilityType type;
        public GameObject[] actions;
    }

    public class PlayerAbilityAuthoring : MonoBehaviour
    {

        public int resetChainAfter;
        public int inputTriggerDuration;
        public PlayerAbility[] abilities;

        private class Baker : Unity.Entities.Baker<PlayerAbilityAuthoring>
        {
            public override void Bake(PlayerAbilityAuthoring authoring)
            {
                var owner = GetEntity(TransformUsageFlags.Dynamic);

                var playerInput = authoring.GetComponent<PlayerInput>();
                if (playerInput != null)
                {
                    AddComponentObject(owner, playerInput);
                }

                var animator = authoring.GetComponent<Animator>();
                if (animator != null)
                {
                    AddComponentObject(owner, animator);
                }

                if (authoring.abilities == null)
                    return;

                for (int i = 0; i < authoring.abilities.Length; i++)
                {
                    var ability = authoring.abilities[i];
                    if (ability.input == null || ability.input.action == null)
                        continue;

                    var entityName = $"{authoring.name} {ability.input.action.name} Input Event";
                    var e = CreateAdditionalEntity(TransformUsageFlags.None, false, entityName);

                    switch (ability.type)
                    {
                        case AbilityType.Simple:
                            AddComponent<ActionSimple>(e);
                            break;
                        case AbilityType.Direct:
                            AddComponent<ActionDirect>(e);
                            break;
                        case AbilityType.Chain:
                            AddComponent(e, new ActionChain()
                            {
                                resetChainDuration = authoring.resetChainAfter
                            });
                            break;
                        case AbilityType.Charge:
                            AddComponent<ActionCharge>(e);
                            break;
                    }

                    AddComponent(e, new ChannelData()
                    {
                        channel = ability.channel,
                        type = ability.channelType
                    });

                    AddComponent(e, new InputEvent()
                    {
                        owner = owner,
                        priority = ability.inputPriority,
                        id = ability.input.action.id,
                        inputResetDuration = authoring.inputTriggerDuration,
                        continuous = ability.continuous
                    });

                    DynamicBuffer<ActionBufferData> acbuffer = AddBuffer<ActionBufferData>(e);
                    if (ability.actions == null)
                        continue;

                    for (int j = 0; j < ability.actions.Length; j++)
                    {
                        if (ability.actions[j] == null)
                            continue;

                        acbuffer.Add(new ActionBufferData()
                        {
                            action = GetEntity(ability.actions[j], TransformUsageFlags.None)
                        });
                    }
                }
            }
        }
    }
}
