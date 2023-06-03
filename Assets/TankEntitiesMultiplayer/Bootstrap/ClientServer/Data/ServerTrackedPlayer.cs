using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [InternalBufferCapacity(32)]
    public struct ServerTrackedPlayer : IBufferElementData
    {
        public Hash128 playerGuid;
        public int playerId; // guid hash

        public Entity networkEntity;

        public bool isLoaded;
    }
}