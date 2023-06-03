using TankEntitiesMultiplayer.Data;
using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerPublicPlayerDataSystem : ISystem
    {
        private Entity _publicDataEntity;
        private EntityQuery _serverTrackedQuery;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CoreReferences>();

            _serverTrackedQuery = SystemAPI.QueryBuilder().WithAll<ServerTrackedPlayer>().Build();
            _serverTrackedQuery.AddChangedVersionFilter(ComponentType.ReadOnly<ServerTrackedPlayer>());

            state.RequireForUpdate(_serverTrackedQuery);
        }

        public void OnUpdate(ref SystemState state)
        {
            if (_publicDataEntity == Entity.Null)
            {
                var references = SystemAPI.GetSingleton<CoreReferences>();
                _publicDataEntity = state.EntityManager.Instantiate(references.connectionPrefab);
            }

            // IsEmpty because we need change version check
            if (!_serverTrackedQuery.IsEmpty)
            {
                var serverTracked = SystemAPI.GetSingletonBuffer<ServerTrackedPlayer>(true).AsNativeArray();
                var publicTracked = SystemAPI.GetSingletonBuffer<PublicPlayerData>();

                publicTracked.Clear();
                foreach (var serverTrackedPlayer in serverTracked)
                {
                    publicTracked.Add(new()
                    {
                        playerId = serverTrackedPlayer.playerId,
                        isLoaded = serverTrackedPlayer.isLoaded
                    });
                }
            }
        }
    }
}