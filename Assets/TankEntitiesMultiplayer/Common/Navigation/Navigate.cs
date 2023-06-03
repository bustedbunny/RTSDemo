using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkit.Messaging;

namespace TankEntitiesMultiplayer
{
    public abstract class NavSignatureBase<T> : ValueMessage<T, bool> where T : NavSignatureBase<T>, new() { }

    public class ScreenNavigationMessage
    {
        public IMessageSignature Signature { get; protected set; }
        public bool IsRoot { get; protected set; }
    }

    public interface IMessageSignature
    {
        public void Unwrap(StrongReferenceMessenger messenger, bool enable);
    }

    public class Navigate<T> : ScreenNavigationMessage where T : ValueMessage<T, bool>, new()
    {
        private static readonly Navigate<T> Instance = new();

        private static readonly MessageSignature SignatureInstance = new();

        private static readonly T MessageInstance = new() { value = true };

        public static ScreenNavigationMessage Message(bool isRoot = false)
        {
            Instance.Signature = SignatureInstance;
            Instance.IsRoot = isRoot;
            return Instance;
        }


        private class MessageSignature : IMessageSignature
        {
            public void Unwrap(StrongReferenceMessenger messenger, bool enable)
            {
                MessageInstance.value = enable;
                messenger.Send(MessageInstance);
            }
        }
    }
}