using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    public struct ClientAuthorization : IComponentData
    {
        public int playerId;
        public int sessionId;
    }
}