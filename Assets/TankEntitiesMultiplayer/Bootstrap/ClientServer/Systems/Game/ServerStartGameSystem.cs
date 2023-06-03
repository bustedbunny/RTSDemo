using System.Collections;
using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Session;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Scenes;
using UnityEngine;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    public partial class ServerStartGameSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<StartGame>();
        }

        protected override void OnStartRunning()
        {
            _loadingRoutine = new(LoadRoutine());
        }

        private RoutineUpdater _loadingRoutine;

        private IEnumerator LoadRoutine()
        {
            var coreReferences = SystemAPI.GetSingleton<CoreReferences>();

            // instantiate loading ghost
            EntityManager.Instantiate(coreReferences.loadingPrefab);

            var testSubSceneToLoad = coreReferences.gameSubScene;
            var sessionSubScenes = SystemAPI.GetSingletonBuffer<SessionSubScene>();
            sessionSubScenes.Add(new() { subScene = testSubSceneToLoad });


            // Change server status to stop modifications to tracked player buffer
            SystemAPI.GetSingletonRW<ServerLobbyData>().ValueRW.status = ServerLobbyStatus.Loading;

            // Wait for all players to load
            {
                var isDone = false;

                while (!isDone)
                {
                    var readyQuery = SystemAPI.QueryBuilder().WithAll<ClientIsReadyRpc, ReceiveRpcCommandRequest>()
                        .Build();

                    isDone = true;

                    var players = SystemAPI.GetSingletonBuffer<ServerTrackedPlayer>().AsNativeArray();

                    foreach (var player in players)
                    {
                        if (!SystemAPI.Exists(player.networkEntity))
                        {
                            Debug.LogError("Disconnected during loading.");
                        }
                    }

                    foreach (var player in players)
                    {
                        isDone &= player.isLoaded;
                    }

                    if (!readyQuery.IsEmptyIgnoreFilter)
                    {
                        foreach (var entity in readyQuery.ToEntityArray(Allocator.Temp))
                        {
                            var req = SystemAPI.GetComponent<ReceiveRpcCommandRequest>(entity);

                            for (var i = 0; i < players.Length; i++)
                            {
                                var player = players[i];
                                if (player.networkEntity == req.SourceConnection)
                                {
                                    player.isLoaded = true;
                                    players[i] = player;
                                    break;
                                }
                            }
                        }

                        EntityManager.DestroyEntity(readyQuery);
                    }

                    yield return null;
                }
            }

            // Load subScene on server itself
            // They are loaded only after client to ensure no desync due to missing ghost on client.

            var testSubSceneToLoadEntity = SceneSystem.LoadSceneAsync(World.Unmanaged, testSubSceneToLoad);
            EntityManager.CreateSingletonBuffer<LoadedSubScenes>();
            var subScenesToLoad = SystemAPI.GetSingletonBuffer<LoadedSubScenes>();
            subScenesToLoad.Add(new()
            {
                scene = testSubSceneToLoadEntity,
                hash = testSubSceneToLoad
            });


            {
                var isDone = false;
                while (!isDone)
                {
                    isDone = true;
                    var toLoad = SystemAPI.GetSingletonBuffer<LoadedSubScenes>();
                    var array = toLoad.AsNativeArray();
                    for (var i = 0; i < array.Length; i++)
                    {
                        var subScene = array[i];
                        if (subScene.isLoaded)
                        {
                            continue;
                        }

                        subScene.isLoaded = SceneSystem.IsSceneLoaded(World.Unmanaged, subScene.scene);
                        array[i] = subScene;
                        isDone &= subScene.isLoaded;
                    }

                    yield return null;
                }
            }

            var timeStamp = SystemAPI.Time.ElapsedTime + 0.5;
            while (SystemAPI.Time.ElapsedTime < timeStamp)
            {
                yield return null;
            }

            // Once everything is done - instantiate trigger
            EntityManager.Instantiate(SystemAPI.GetSingleton<CoreReferences>().gameIsStartedPrefab);
        }

        protected override void OnUpdate()
        {
            _loadingRoutine.Update();
        }

        protected override void OnStopRunning()
        {
            // remove loading ghost so clients can unload their resources
            EntityManager.DestroyEntity(SystemAPI.QueryBuilder().WithAll<SessionSubScene>().Build());
            EntityManager.DestroyEntity(SystemAPI.QueryBuilder().WithAll<LoadedSubScenes>().Build());
        }
    }
}