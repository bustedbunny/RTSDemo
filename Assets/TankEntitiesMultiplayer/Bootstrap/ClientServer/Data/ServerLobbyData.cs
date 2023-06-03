using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    public struct ServerLobbyData : IComponentData
    {
        public ServerLobbyStatus status;

        public Hash128 sessionGuid;
        public int sessionId;
    }

    public enum ServerLobbyStatus
    {
        Lobby,
        Loading,
        Gaming
    }
}