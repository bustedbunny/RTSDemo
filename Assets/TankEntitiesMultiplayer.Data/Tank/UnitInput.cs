using Unity.Entities;
using Unity.Mathematics;

namespace TankEntitiesMultiplayer.Data.Tank
{
    public struct UnitInput : IComponentData
    {
        public int horizontal;
        public int vertical;

        public float3 aimOrigin;
        public float3 aimDirection;

        public bool shoot;
    }
}