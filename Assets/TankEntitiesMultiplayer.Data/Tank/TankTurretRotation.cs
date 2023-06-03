using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Data.Tank
{
    [GhostComponent]
    public struct TankTurretRotation : IComponentData
    {
        [GhostField(Quantization = 1000)] public float turretRotation;
        [GhostField(Quantization = 1000)] public float cannonRotation;
    }
}