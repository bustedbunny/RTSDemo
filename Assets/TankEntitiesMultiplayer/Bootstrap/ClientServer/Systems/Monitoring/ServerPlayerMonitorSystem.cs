using TankEntitiesMultiplayer.Data;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerPlayerMonitorSystem : ISystem
    {
        private struct InitializedConnection : IComponentData { }

        private EntityQuery _addConQuery;
        private EntityQuery _initConQuery;
        private EntityQuery _dcQuery;
        private Entity _publicDataEntity;

        public void OnCreate(ref SystemState state)
        {
            _addConQuery = SystemAPI.QueryBuilder().WithNone<ConnectionState>().WithAll<AuthorizedPlayer>().Build();
            _initConQuery = SystemAPI.QueryBuilder().WithNone<InitializedConnection>()
                .WithAll<AuthorizedPlayer, NetworkId>()
                .Build();
            _dcQuery = SystemAPI.QueryBuilder().WithNone<AuthorizedPlayer>().WithAll<ConnectionState>().Build();

            state.RequireAnyForUpdate(_addConQuery, _initConQuery, _dcQuery);
            state.RequireForUpdate<CoreReferences>();
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.EntityManager.AddComponent<ConnectionState>(_addConQuery);


            var worldName = state.WorldUnmanaged.Name;
            var ids = _initConQuery.ToComponentDataArray<NetworkId>(state.WorldUpdateAllocator);

            foreach (var networkId in ids)
            {
                Debug.Log($"[{worldName}] New connection ID:{networkId.Value}");
            }


            foreach (var connection in _dcQuery.ToComponentDataArray<ConnectionState>(state.WorldUpdateAllocator))
            {
                Debug.Log(
                    $"[{worldName}] Connection disconnected ID:{connection.NetworkId} Reason: {DisconnectReasonEnumToString.Convert((int)connection.DisconnectReason)}");
            }

            state.EntityManager.AddComponent<InitializedConnection>(_initConQuery);
            state.EntityManager.RemoveComponent<ConnectionState>(_dcQuery);
        }
    }
}