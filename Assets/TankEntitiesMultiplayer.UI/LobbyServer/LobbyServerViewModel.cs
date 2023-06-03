using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMToolkit;
using TankEntitiesMultiplayer.Bootstrap;
using TankEntitiesMultiplayer.Bootstrap.Management;

namespace TankEntitiesMultiplayer.UI
{
    public partial class LobbyServerViewModel : ViewModel
    {
        [ObservableProperty] private string _port;

        private bool CanStartGame()
        {
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanStartGame))]
        private void StartGame()
        {
            ClientServerState.Server.EntityManager.CreateSingleton<StartGame>();
        }
    }
}