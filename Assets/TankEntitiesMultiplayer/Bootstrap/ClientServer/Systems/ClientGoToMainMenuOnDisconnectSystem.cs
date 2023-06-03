using Cysharp.Threading.Tasks;
using TankEntitiesMultiplayer.Bootstrap.Management;
using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial class ClientGoToMainMenuOnDisconnectSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<NetworkId>();
        }

        protected override void OnStopRunning()
        {
            UniTask.Create(async () =>
            {
                await UniTask.Yield();
                Game.SetState<MainMenuState>();
            });
        }

        protected override void OnUpdate() { }
    }
}