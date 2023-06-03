using Unity.NetCode;

namespace TankEntitiesMultiplayer.Bootstrap
{
    public struct AuthorizationRpc : IRpcCommand
    {
        public CommandType type;
        public int playerId;
        public int sessionId;
    }
}