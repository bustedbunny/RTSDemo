using CommunityToolkit.Mvvm.Messaging;
using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Bootstrap.Management
{
    public class MainMenuState : IWorldState
    {
        public void Start()
        {
            Local = ClientServerBootstrap.CreateLocalWorld(NetCodeBootstrap.DefaultWorldName);
            Game.Messenger.Send(Navigate<MainMenu>.Message(true));
        }

        public static World Local { get; private set; }

        public void Dispose()
        {
            Local.Dispose();
        }
    }
}