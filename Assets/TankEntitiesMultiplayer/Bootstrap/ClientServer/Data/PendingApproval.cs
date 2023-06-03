using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    public struct PendingApproval : IComponentData
    {
        public double kickTime;
    }
}