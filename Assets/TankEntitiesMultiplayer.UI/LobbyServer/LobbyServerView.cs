using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging;
using TankEntitiesMultiplayer.Bootstrap;
using UnityEngine;
using UnityEngine.UIElements;

namespace TankEntitiesMultiplayer.UI
{
    public class LobbyServerView : NavBaseView<GameLobby>,
        IRecipient<PublicPlayerDataMessage>,
        IRecipient<ConnectionTypeMessage>
    {
        [SerializeField] private VisualTreeAsset _playerTemplate;

        private VisualElement _startButton;

        private VisualElement _listCont;
        private readonly List<Label> _elements = new();

        protected override void OnInit()
        {
            base.OnInit();

            _startButton = RootVisualElement.Q("button-start");
            _listCont = RootVisualElement.Q("list-player");
        }

        public void Receive(ConnectionTypeMessage message)
        {
            var value = message.value == ConnectionType.Client ? DisplayStyle.None : DisplayStyle.Flex;
            _startButton.style.display = value;
        }

        public void Receive(PublicPlayerDataMessage message)
        {
            if (_listCont.childCount != message.value.Length)
            {
                _listCont.Clear();
                _elements.Clear();

                for (var i = 0; i < message.value.Length; i++)
                {
                    var label = new Label();
                    _listCont.Add(label);
                    _elements.Add(label);
                }
            }

            for (int i = 0; i < message.value.Length; i++)
            {
                var label = _elements[i];
                var data = message.value[i];
                label.text = $"Connection id: {data.playerId}";
            }
        }
    }
}