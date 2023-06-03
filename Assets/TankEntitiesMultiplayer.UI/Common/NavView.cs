using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkit;
using UnityEngine;
using UnityEngine.UIElements;

namespace TankEntitiesMultiplayer.UI
{
    public abstract partial class NavBaseView<T> : BaseView, IRecipient<T> where T : NavSignatureBase<T>, new()
    {
        protected override void OnInit()
        {
            RootVisualElement.RegisterCallback<NavigationCancelEvent>(Callback);
        }

        private void Callback(NavigationCancelEvent evt)
        {
            Debug.Log("Back");
            Messenger.Send(NavigationBack.Message());
        }

        public void Receive(T message) => enabled = message.value;
    }
}