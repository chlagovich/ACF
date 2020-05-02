using UnityEngine;
using Unity.Entities;

namespace SquareBattle
{
    public class ChannelMixerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public ActionChannel[] channels;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            DynamicBuffer<ChannelMixerState> channelMixer = dstManager.AddBuffer<ChannelMixerState>(entity);
            
            for (int i = 0; i < channels.Length; i++)
            {
                channelMixer.Add(new ChannelMixerState()
                {
                    action = Entity.Null,
                    channel = channels[i],
                    lastNbrFrames = 0
                });
            }

            DynamicBuffer<ChannelEntry> channelEntry = dstManager.AddBuffer<ChannelEntry>(entity);
            for (int i = 0; i < channels.Length; i++)
            {
                channelEntry.Add(new ChannelEntry()
                {
                    action = Entity.Null,
                    channel = channels[i],
                    source = Entity.Null
                });
            }
        }
    }
}