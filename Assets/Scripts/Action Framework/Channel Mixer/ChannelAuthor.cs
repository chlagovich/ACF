using System;
using Unity.Entities;
using UnityEngine;

public class ChannelAuthor : MonoBehaviour
{
    private class Baker : Unity.Entities.Baker<ChannelAuthor>
    {
        public override void Bake(ChannelAuthor authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            DynamicBuffer<ChannelsBuffer> acbuffer = AddBuffer<ChannelsBuffer>(entity);

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
}
