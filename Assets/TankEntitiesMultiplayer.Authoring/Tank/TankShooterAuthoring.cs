using TankEntitiesMultiplayer.Data.Tank;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring.Tank
{
    public class TankShooterAuthoring : MonoBehaviour
    {
        public GameObject ammoPrefab;
        public float initialVelocity;
        public GameObject shootingPosition;

        public class TankShooterBaker : Baker<TankShooterAuthoring>
        {
            public override void Bake(TankShooterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new TankShooter
                    {
                        ammoPrefab = GetEntity(authoring.ammoPrefab, TransformUsageFlags.Dynamic),
                        initialVelocity = authoring.initialVelocity,
                        shootingPosition = GetEntity(authoring.shootingPosition, TransformUsageFlags.Dynamic)
                    });
            }
        }
    }
}