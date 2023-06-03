using System;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Cysharp.Threading.Tasks;
using MVVMToolkit.DependencyInjection;
using TankEntitiesMultiplayer.Bootstrap.Management;
using UnityEngine;

namespace TankEntitiesMultiplayer
{
    public static class Game
    {
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Init()
        {
            UnityEditor.EditorApplication.playModeStateChanged += change =>
            {
                if (change is UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    _stateController = null;
                }
                else if (change is UnityEditor.PlayModeStateChange.ExitingPlayMode)
                {
                    _stateController?.Dispose();
                }
            };
        }
        #endif


        public static void Initialize(StrongReferenceMessenger messenger, ServiceProvider sp)
        {
            Messenger = messenger;
            Services = sp;
            _stateController = new();
        }

        private static StateController _stateController;
        public static StrongReferenceMessenger Messenger { get; private set; }
        public static ServiceProvider Services { get; private set; }

        public static void SetState(IWorldState newState) => _stateController.LoadState(newState);

        public static void SetState<T>() where T : IWorldState, new() => SetState(new T());
    }

    internal class StateController : IDisposable
    {
        private UniTask _currentTask = UniTask.CompletedTask;
        private IWorldState _current;

        private readonly CancellationTokenSource _cts = new();

        public void LoadState(IWorldState state)
        {
            #if UNITY_EDITOR
            if (_wantsToQuit)
            {
                return;
            }
            #endif

            var isLoaded = _currentTask.Status is not UniTaskStatus.Pending;

            if (isLoaded)
            {
                _current?.Dispose();

                _current = state;
                state.Start();
                StartAsyncLoad(state);
            }
            else
            {
                Debug.LogWarning($"Trying to load state {state.GetType().Name}, before previous is loaded.");

                UniTask.Create(async () =>
                {
                    await _currentTask;

                    _current.Dispose();


                    _current = state;
                    state.Start();
                    StartAsyncLoad(state);
                });
            }
        }

        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Init()
        {
            UnityEditor.EditorApplication.playModeStateChanged += change =>
            {
                if (change == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    _wantsToQuit = false;
                }
                else if (change == UnityEditor.PlayModeStateChange.ExitingPlayMode)
                {
                    _wantsToQuit = true;
                }
            };
        }

        private static bool _wantsToQuit;
        #endif

        private void StartAsyncLoad(IWorldState state)
        {
            try
            {
                _currentTask = state.AsyncStart(_cts.Token);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _current.Dispose();
        }
    }
}