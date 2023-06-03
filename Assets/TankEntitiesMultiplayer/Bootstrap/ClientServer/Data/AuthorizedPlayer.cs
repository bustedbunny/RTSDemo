using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    public struct AuthorizedPlayer : ICleanupComponentData
    {
        public int playerId;
    }
}