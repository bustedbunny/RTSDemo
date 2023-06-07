using Unity.Entities;
using Unity.Mathematics;

namespace TankEntitiesMultiplayer.Data.Tank
{
    public struct UnitInput : IComponentData
    {
        public int horizontal;
        public int vertical;

        public bool hasTarget;
        public Entity enemy;
        public float3 lookPosition;

        public bool shoot;
    }
}