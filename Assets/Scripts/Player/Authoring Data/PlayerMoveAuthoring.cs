using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace SquareBattle
{
    public class PlayerMoveAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public InputActionReference input;
        public ActionChannel channel;
        public GameObject action;


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            var e = dstManager.CreateEntity();
#if UNITY_EDITOR
            dstManager.SetName(e, dstManager.GetName(entity) + " " + input.action.name + " Input Event");
#endif

            dstManager.AddComponentData(e, new InputEvent()
            {
                owner = entity,
                priority = 0,
                id = input.action.id
            });
            dstManager.AddComponentData(e, new ChannelData() { channel = channel });
            //dstManager.AddComponentData(e, new ActionDirect()
            //{
            //    action = conversionSystem.GetPrimaryEntity(action)
            //});

        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(action);
        }
    }
}