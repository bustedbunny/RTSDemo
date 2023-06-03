using System.Collections.Generic;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Cysharp.Threading.Tasks;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Scenes;
using UnityEngine;

namespace TankEntitiesMultiplayer.Bootstrap.Management
{
    public class ClientServerState : IWorldState
    {
        public ClientServerState(ushort port)
        {
            _port = port;
        }

        public void Dispose()
        {
            var clientServerWorlds = new List<World>();
            foreach (var world in World.All)
            {
                if (world.IsClient() || world.IsServer())
                    clientServerWorlds.Add(world);
            }

            foreach (var world in clientServerWorlds)
                world.Dispose();

            Server = null;
            Client = null;
        }

        private readonly ushort _port;
        public static World Server { get; private set; }
        public static World Client { get; private set; }

        public async UniTask AsyncStart(CancellationToken ct)
        {
            if (ClientServerBootstrap.RequestedPlayType != ClientServerBootstrap.PlayType.ClientAndServer)
            {
                throw new(
                    $"Creating client/server worlds is not allowed if playmode is set to {ClientServerBootstrap.RequestedPlayType}");
            }

            Server = ClientServerBootstrap.CreateServerWorld("ServerWorld");
            Client = ClientServerBootstrap.CreateClientWorld("ClientWorld");
            World.DefaultGameObjectInjectionWorld = Server;

            var list = new List<(Entity, World)>();
            foreach (var subScene in Object.FindObjectsByType<SubScene>(FindObjectsSortMode.None))
            {
                if (subScene.AutoLoadScene)
                {
                    var server = SceneSystem.LoadSceneAsync(Server.Unmanaged, subScene.SceneGUID);
                    var client = SceneSystem.LoadSceneAsync(Client.Unmanaged, subScene.SceneGUID);
                    Server.EntityManager.AddComponentObject(server, subScene);
                    Client.EntityManager.AddComponentObject(client, subScene);
                    list.Add((server, Server));
                    list.Add((client, Client));
                }
            }

            while (list.Count > 0)
            {
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var (e, world) = list[i];
                    if (SceneSystem.IsSceneLoaded(world.Unmanaged, e))
                    {
                        list.RemoveAt(i);
                    }
                }

                await UniTask.Yield(ct);
            }


            var compType = ComponentType.ReadWrite<NetworkStreamDriver>();
            var ep = NetworkEndpoint.AnyIpv4.WithPort(_port);
            {
                using var serverDriverQuery = Server.EntityManager.CreateEntityQuery(compType);
                serverDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(ep);
            }
            ep = NetworkEndpoint.LoopbackIpv4.WithPort(_port);

            using var drvQuery = Client.EntityManager.CreateEntityQuery(compType);
            var connection = drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW
                .Connect(Client.EntityManager, ep);
            Client.EntityManager.AddComponent<AuthorizeConnectionAsPlayer>(connection);

            Game.Messenger.Send(Navigate<GameLobby>.Message(true));
        }
    }
}