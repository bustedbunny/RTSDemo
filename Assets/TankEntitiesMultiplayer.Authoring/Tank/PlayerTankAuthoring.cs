using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring.Tank
{
    public class PlayerTankAuthoring : MonoBehaviour
    {
        public class PlayerTankAuthoringBaker : Baker<PlayerTankAuthoring>
        {
            public override void Bake(PlayerTankAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitInput());
                AddComponent(entity, new ControllableUnit());
            }
        }
    }
}