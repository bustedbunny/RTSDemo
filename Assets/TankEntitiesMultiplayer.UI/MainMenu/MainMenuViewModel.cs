using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkit;
using MVVMToolkit.Messaging;
using TankEntitiesMultiplayer.Bootstrap.Management;
using Unity.Networking.Transport;
using UnityEngine;

namespace TankEntitiesMultiplayer.UI
{
    public partial class MainMenuViewModel : ViewModel
    {
        [ObservableProperty] private string _port = "7979";
        [ObservableProperty] private string _address = "127.0.0.1";

        [RelayCommand]
        private void LobbyServer()
        {
            Game.Messenger.Send(ConnectionTypeMessage.Message(ConnectionType.Server));
            Game.SetState(new ClientServerState(ParsePortOrDefault(Port)));
        }

        [RelayCommand]
        private void ClientConnect()
        {
            Game.Messenger.Send(ConnectionTypeMessage.Message(ConnectionType.Client));
            Game.SetState(new ClientState(NetworkEndpoint.Parse(Address, ParsePortOrDefault(Port))));
        }

        private const ushort NetworkPort = 7979;

        // The port will be set to whatever is parsed, otherwise the default port of k_NetworkPort
        private static ushort ParsePortOrDefault(string s)
        {
            if (!ushort.TryParse(s, out var port))
            {
                Debug.LogWarning($"Unable to parse port, using default port {NetworkPort}");
                return NetworkPort;
            }

            return port;
        }
    }

    public class ConnectionTypeMessage : ValueMessage<ConnectionTypeMessage, ConnectionType> { }

    public enum ConnectionType
    {
        Client,
        Server
    }
}