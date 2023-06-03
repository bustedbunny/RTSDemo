using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using TankEntitiesMultiplayer.Bootstrap;
using UnityEngine;
using UnityEngine.UIElements;

namespace TankEntitiesMultiplayer.UI.LobbyServerLoading
{
    public partial class LobbyLoadingView : NavBaseView<LobbyLoading>, IRecipient<PublicPlayerDataMessage>

    {
        [SerializeField] private VisualTreeAsset _playerTemplate;

        protected override void OnInit()
        {
            base.OnInit();
            _container = RootVisualElement.Q("player-list");
        }

        private int _currentCount;
        private VisualElement _container;

        private readonly List<StatusElement> _elements = new();

        public void Receive(PublicPlayerDataMessage message)
        {
            if (message.value.Length != _currentCount)
            {
                _container.Clear();
                _elements.Clear();

                for (var i = 0; i < message.value.Length; i++)
                {
                    var root = _playerTemplate.Instantiate();
                    var status = new StatusElement(root.Q<Label>("label-player"), root.Q<Label>("label-progress"),
                        root);
                    _elements.Add(status);
                    _container.Add(root);
                }
            }


            for (var i = 0; i < message.value.Length; i++)
            {
                var playerConnection = message.value[i];
                var status = _elements[i];
                status.Id = playerConnection.playerId;
                status.Status = playerConnection.isLoaded;
            }
        }

        private partial class StatusElement : ObservableObject
        {
            public StatusElement(Label idLabel, Label statusLabel, VisualElement root)
            {
                _idLabel = idLabel;
                _statusLabel = statusLabel;
                _root = root;

                OnStatusChanged(_status);
            }

            [ObservableProperty] private int _id;

            partial void OnIdChanged(int value)
            {
                _idLabel.text = $"Player ID: {value}";
            }

            [ObservableProperty] private bool _status;

            partial void OnStatusChanged(bool value)
            {
                _statusLabel.text = value ? "Ready" : "Loading";
            }

            private readonly Label _idLabel;
            private readonly Label _statusLabel;
            private readonly VisualElement _root;
        }
    }
}