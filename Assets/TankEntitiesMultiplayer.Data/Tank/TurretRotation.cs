using Unity.Entities;
using Unity.Mathematics;

namespace TankEntitiesMultiplayer.Data.Tank
{
    public struct TankWithTurret : IComponentData
    {
        public Entity turret;
        public Entity cannon;
        public float2 cannonConstraints;
    }
}