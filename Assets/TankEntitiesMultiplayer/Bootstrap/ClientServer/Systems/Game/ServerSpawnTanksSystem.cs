using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Session;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerSpawnTanksSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameIsStarted>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            var players = SystemAPI.GetSingletonBuffer<ServerTrackedPlayer>(true).AsNativeArray();
            var tankPrefab = SystemAPI.GetSingleton<TankReferences>().t34;

            foreach (var player in players)
            {
                var newTank = state.EntityManager.Instantiate(tankPrefab);
                SystemAPI.SetComponent(newTank, new ControllableUnit
                {
                    playedId = player.playerId
                });
            }
        }

        public void OnStopRunning(ref SystemState state) { }
    }
}