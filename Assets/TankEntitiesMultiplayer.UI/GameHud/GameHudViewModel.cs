using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkit;

namespace TankEntitiesMultiplayer.UI
{
    public partial class GameHudViewModel : ViewModel
    {
        [RelayCommand]
        private void Menu()
        {
            Messenger.Send(Navigate<GameMenu>.Message());
        }
    }
}