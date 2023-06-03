using System.Collections;
using CommunityToolkit.Mvvm.Messaging;
using TankEntitiesMultiplayer.Data.Session;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Scenes;
using UnityEngine;


namespace TankEntitiesMultiplayer.Bootstrap
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    public partial class ClientStartGameSystem : SystemBase
    {
        private EntityQuery _query;
        private RoutineUpdater _routine;

        protected override void OnCreate()
        {
            _query = SystemAPI.QueryBuilder().WithAll<SessionSubScene>().Build();
            RequireForUpdate(_query);
            EntityManager.CreateSingletonBuffer<LoadedSubScenes>();
        }

        protected override void OnStartRunning()
        {
            var toLoad = SystemAPI.GetSingletonBuffer<SessionSubScene>(true).AsNativeArray();
            var list = new NativeArray<Entity>(toLoad.Length, Allocator.Persistent);
            for (var i = 0; i < toLoad.Length; i++)
            {
                var sessionSubScene = toLoad[i];
                list[i] = SceneSystem.LoadSceneAsync(World.Unmanaged, sessionSubScene.subScene);
            }


            var subScenes = SystemAPI.GetSingletonBuffer<LoadedSubScenes>();
            for (var i = 0; i < toLoad.Length; i++)
            {
                subScenes.Add(new()
                {
                    scene = list[i],
                });
            }

            if (!World.IsThinClient())
            {
                Game.Messenger.Send(Navigate<LobbyLoading>.Message(true));
            }

            _routine = new(LoadSubScenesRoutine(list));
        }

        private IEnumerator LoadSubScenesRoutine(NativeArray<Entity> list)
        {
            var isDone = false;
            while (!isDone)
            {
                isDone = true;
                for (var i = 0; i < list.Length; i++)
                {
                    var subScene = list[i];
                    if (subScene == Entity.Null)
                    {
                        continue;
                    }

                    var isLoaded = SceneSystem.IsSceneLoaded(World.Unmanaged, subScene);
                    if (isLoaded)
                    {
                        list[i] = Entity.Null;
                    }
                    else
                    {
                        isDone = false;
                    }
                }

                yield return null;
            }

            var e = EntityManager.CreateEntity();
            var typeSet = ComponentTypeSetUtility.Create<ClientIsReadyRpc, SendRpcCommandRequest>();
            EntityManager.AddComponent(e, typeSet);

            var startQuery = SystemAPI.QueryBuilder().WithAll<GameIsStarted>().Build();
            while (startQuery.IsEmptyIgnoreFilter)
            {
                yield return null;
            }

            if (!World.IsThinClient())
            {
                Game.Messenger.Send(Navigate<GameHud>.Message(true));
            }
        }

        protected override void OnUpdate()
        {
            _routine.Update();
        }

        protected override void OnStopRunning()
        {
            var loadedSubScenes = SystemAPI.GetSingletonBuffer<LoadedSubScenes>();
            loadedSubScenes.Clear();
            foreach (var loadedSubScene in loadedSubScenes.ToNativeArray(WorldUpdateAllocator))
            {
                SceneSystem.UnloadScene(World.Unmanaged, loadedSubScene.scene);
            }
        }
    }
}