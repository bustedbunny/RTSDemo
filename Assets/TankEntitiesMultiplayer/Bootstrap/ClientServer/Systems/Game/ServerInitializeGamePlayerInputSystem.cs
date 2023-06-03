using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Session;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerInitializeGamePlayerInputSystem : ISystem
    {
        private EntityQuery _query;

        private struct Initialized : IComponentData { }

        public void OnCreate(ref SystemState state)
        {
            _query = SystemAPI.QueryBuilder().WithAll<NetworkId>().WithNone<Initialized>().Build();
            state.RequireForUpdate<GameIsStarted>();
            state.RequireForUpdate(_query);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var prefab = SystemAPI.GetSingleton<CoreReferences>().runtimePrefab;

            var entities = _query.ToEntityArray(state.WorldUpdateAllocator);
            var connections = _query.ToComponentDataArray<NetworkId>(state.WorldUpdateAllocator);
            for (var i = 0; i < connections.Length; i++)
            {
                var entity = entities[i];
                var networkId = connections[i];

                var player = SystemAPI.GetComponent<AuthorizedPlayer>(entity);

                var playerInput = state.EntityManager.Instantiate(prefab);
                SystemAPI.SetComponent(playerInput, new GhostOwner { NetworkId = networkId.Value });
                SystemAPI.SetComponent(playerInput, new PlayerId { playerId = player.playerId });
                var leg = SystemAPI.GetBuffer<LinkedEntityGroup>(entity);
                leg.Add(new() { Value = playerInput });
            }

            state.EntityManager.AddComponent<Initialized>(_query);
        }
    }
}