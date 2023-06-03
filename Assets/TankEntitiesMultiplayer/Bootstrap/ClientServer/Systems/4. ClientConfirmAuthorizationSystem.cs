using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial class ClientConfirmAuthorizationSystem : SystemBase
    {
        private EntityQuery _receiveQuery;

        protected override void OnCreate()
        {
            _receiveQuery = SystemAPI.QueryBuilder().WithAll<AuthorizationRpc, ReceiveRpcCommandRequest>()
                .Build();

            RequireAnyForUpdate(_receiveQuery);
        }

        protected override void OnUpdate()
        {
            var auth = SystemAPI.GetSingleton<AuthorizationRpc>();

            if (auth.type is CommandType.ServerConnect)
            {
                if (!SystemAPI.HasSingleton<ClientAuthorization>())
                {
                    EntityManager.CreateSingleton<ClientAuthorization>();
                }

                SystemAPI.SetSingleton(new ClientAuthorization
                {
                    playerId = auth.playerId,
                    sessionId = auth.sessionId
                });
            }

            EntityManager.DestroyEntity(_receiveQuery);
            var networkQuery = SystemAPI.QueryBuilder().WithAll<NetworkId>().Build();
            EntityManager.AddComponent<NetworkStreamInGame>(networkQuery);
        }
    }
}