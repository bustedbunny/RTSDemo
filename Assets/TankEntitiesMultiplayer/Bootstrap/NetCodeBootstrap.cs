using System;
using Cysharp.Threading.Tasks;
using MVVMToolkit;
using TankEntitiesMultiplayer.Bootstrap.Management;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TankEntitiesMultiplayer.Bootstrap
{
// Create a custom bootstrap, which enables auto-connect.
// The bootstrap can also be used to configure other settings as well as to
// manually decide which worlds (client and server) to create based on user input
    [UnityEngine.Scripting.Preserve]
    public class NetCodeBootstrap : ClientServerBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            Game.Initialize(new(), new());
            try
            {
                UniTask.Create(async () =>
                {
                    _ = new NavigationManager(Game.Messenger);
                    await Object.FindObjectOfType<UIRoot>().Initialize(Game.Messenger, Game.Services);
                    DefaultWorldInitialization.Initialize("DefaultWorld");
                });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        public override bool Initialize(string defaultWorldName)
        {
            DefaultWorldName = defaultWorldName;
            Game.SetState(new MainMenuState());
            return true;


            // AutoConnectPort = 7979; // Enabled auto connect
            // return base.Initialize(defaultWorldName); // Use the regular bootstrap
        }

        public static string DefaultWorldName { get; private set; }
    }
}