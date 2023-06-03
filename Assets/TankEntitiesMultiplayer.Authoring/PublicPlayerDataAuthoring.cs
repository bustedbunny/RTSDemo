using TankEntitiesMultiplayer.Data;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring
{
    public class PublicPlayerDataAuthoring : MonoBehaviour
    {
        public class PlayerConnectionAuthoringBaker : Baker<PublicPlayerDataAuthoring>
        {
            public override void Bake(PublicPlayerDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.ManualOverride);
                AddBuffer<PublicPlayerData>(entity);
            }
        }
    }
}