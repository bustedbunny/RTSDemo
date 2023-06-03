using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial class ServerAuthorizedPlayerDisconnectionSystem : SystemBase
    {
        private EntityQuery _disconnectedQuery;

        protected override void OnCreate()
        {
            _disconnectedQuery = SystemAPI.QueryBuilder().WithAll<AuthorizedPlayer>().WithNone<NetworkId>().Build();
            RequireForUpdate(_disconnectedQuery);
        }

        protected override void OnUpdate()
        {
            var serverData = SystemAPI.GetSingleton<ServerLobbyData>();

            var entities = _disconnectedQuery.ToEntityArray(Allocator.Temp);

            // Only remove player tracking if it's a lobby
            if (serverData.status is ServerLobbyStatus.Lobby)
            {
                var players = SystemAPI.GetSingletonBuffer<ServerTrackedPlayer>();
                foreach (var entity in entities)
                {
                    var player = SystemAPI.GetComponent<AuthorizedPlayer>(entity);
                    for (var i = 0; i < players.Length; i++)
                    {
                        var trackedPlayer = players[i];
                        if (player.playerId == trackedPlayer.playerId)
                        {
                            players.RemoveAt(i);
                            break;
                        }
                    }
                }
            }


            EntityManager.RemoveComponent<AuthorizedPlayer>(_disconnectedQuery);
        }
    }
}