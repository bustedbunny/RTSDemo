using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct ClientAuthorizationSystem : ISystem
    {
        private ComponentTypeSet _types;
        private EntityQuery _networkQuery;

        private struct AuthIsSent : IComponentData { }

        public void OnCreate(ref SystemState state)
        {
            _networkQuery = SystemAPI.QueryBuilder().WithAll<NetworkId, AuthorizeConnectionAsPlayer>()
                .WithNone<AuthIsSent>().Build();
            state.RequireForUpdate(_networkQuery);

            _types = ComponentTypeSetUtility.Create<AuthorizationRpc, SendRpcCommandRequest>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var req = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponent(req, _types);

            var rpcData = new AuthorizationRpc
            {
                type = CommandType.PlayerConnect
            };

            if (SystemAPI.TryGetSingleton(out ClientAuthorization auth))
            {
                rpcData.type = CommandType.PlayerReconnect;
                rpcData.playerId = auth.playerId;
                rpcData.sessionId = auth.sessionId;
            }

            Debug.Log($"{state.World.Name} sends auth: {rpcData.type}, {rpcData.playerId}, {rpcData.sessionId}.");

            SystemAPI.SetComponent(req, rpcData);
            state.EntityManager.AddComponent<AuthIsSent>(_networkQuery);
        }
    }
}