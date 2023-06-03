using TankEntitiesMultiplayer.Data;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring
{
    public class PlayerInputAuthoring : MonoBehaviour
    {
        public class PlayerInputBaker : Baker<PlayerInputAuthoring>
        {
            public override void Bake(PlayerInputAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.ManualOverride);
                AddComponent(entity, new PlayerInput());
                AddComponent(entity, new PlayerId());
            }
        }
    }
}