using Unity.Entities;

namespace TankEntitiesMultiplayer.Data.Tank
{
    public struct TankShooter : IComponentData
    {
        public Entity ammoPrefab;
        public float initialVelocity;

        public Entity shootingPosition;
    }
}