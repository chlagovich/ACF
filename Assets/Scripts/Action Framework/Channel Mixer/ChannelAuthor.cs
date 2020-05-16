using System;
using Unity.Entities;
using UnityEngine;


public class ChannelAuthor : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        DynamicBuffer<ChannelsBuffer> acbuffer = dstManager.AddBuffer<ChannelsBuffer>(entity);
        var names = Enum.GetNames(typeof(Channel));
        for (int j = 1; j < names.Length; j++)
        {
            var b = new ChannelsBuffer()
            {
                channel = (Channel)Enum.Parse(typeof(Channel), names[j]),
                blocked = false
            };
            acbuffer.Add(b);
        }
    }
}