using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring.Tank
{
    public class TurretRotationAuthoring : MonoBehaviour
    {
        public GameObject turret;
        public GameObject cannon;

        public float2 cannonConstraints;

        public class CannonRotationBaker : Baker<TurretRotationAuthoring>
        {
            public override void Bake(TurretRotationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new TankWithTurret
                {
                    turret = GetEntity(authoring.turret, TransformUsageFlags.Dynamic),
                    cannon = GetEntity(authoring.cannon, TransformUsageFlags.Dynamic),
                    cannonConstraints = math.radians(authoring.cannonConstraints)
                });
                AddComponent<TankTurretRotation>(entity);
            }
        }
    }
}