using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkit;
using TankEntitiesMultiplayer.Bootstrap.Management;

namespace TankEntitiesMultiplayer.UI
{
    public partial class GameMenuViewModel : ViewModel
    {
        [RelayCommand]
        private void Back() => Messenger.Send(NavigationBack.Message());


        [RelayCommand]
        private void Disconnect()
        {
            Game.SetState<MainMenuState>();
        }
    }
}