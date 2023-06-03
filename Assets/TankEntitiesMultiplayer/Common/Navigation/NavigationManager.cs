using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging;

namespace TankEntitiesMultiplayer
{
    public class NavigationManager : IRecipient<ScreenNavigationMessage>, IRecipient<NavigationBack>
    {
        public NavigationManager(StrongReferenceMessenger messenger)
        {
            _messenger = messenger;
            _messenger.RegisterAll(this);
        }

        private readonly StrongReferenceMessenger _messenger;
        private readonly Stack<IMessageSignature> _stack = new();


        private IMessageSignature _root;

        public void Receive(ScreenNavigationMessage message)
        {
            if (message.IsRoot)
            {
                while (_stack.TryPop(out var result))
                {
                    result.Unwrap(_messenger, false);
                }

                _root?.Unwrap(_messenger, false);

                _root = message.Signature;
                _root.Unwrap(_messenger, true);
                return;
            }

            _stack.Push(message.Signature);
            message.Signature.Unwrap(_messenger, true);
        }

        public void Receive(NavigationBack message)
        {
            if (_stack.TryPop(out var result))
            {
                result.Unwrap(_messenger, false);
            }
        }
    }
}