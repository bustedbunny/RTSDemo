using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TankEntitiesMultiplayer.Bootstrap.Management
{
    public interface IWorldState : IDisposable
    {
        public void Start() { }

        public UniTask AsyncStart(CancellationToken ct) => UniTask.CompletedTask;
    }
}