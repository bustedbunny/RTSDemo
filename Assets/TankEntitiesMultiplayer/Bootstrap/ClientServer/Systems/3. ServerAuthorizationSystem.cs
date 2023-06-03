using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Hash128 = Unity.Entities.Hash128;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial class ServerAuthorizationSystem : SystemBase
    {
        private EntityQuery _authorizationQuery;
        private ComponentTypeSet _serverSendAuthTypeSet;

        protected override void OnCreate()
        {
            var playerStorageSingleton = EntityManager.CreateEntity();
            var storageTypes = ComponentTypeSetUtility.Create<ServerLobbyData, ServerTrackedPlayer>();
            EntityManager.AddComponent(playerStorageSingleton, storageTypes);

            // Generate session guid per server world
            var sessionGuid = new Hash128(Guid.NewGuid().ToString());
            SystemAPI.SetComponent(playerStorageSingleton, new ServerLobbyData
            {
                status = ServerLobbyStatus.Lobby,
                sessionGuid = sessionGuid,
                sessionId = sessionGuid.GetHashCode(),
            });


            _authorizationQuery = SystemAPI.QueryBuilder().WithAll<AuthorizationRpc, ReceiveRpcCommandRequest>()
                .Build();
            RequireAnyForUpdate(_authorizationQuery);

            _serverSendAuthTypeSet = ComponentTypeSetUtility.Create<AuthorizationRpc, SendRpcCommandRequest>();
        }

        protected override void OnUpdate()
        {
            var lobbyData = SystemAPI.GetSingleton<ServerLobbyData>();
            var requests = _authorizationQuery.ToEntityArray(Allocator.Temp);

            if (lobbyData.status is not ServerLobbyStatus.Lobby)
            {
                foreach (var entity in requests)
                {
                    var auth = SystemAPI.GetComponent<AuthorizationRpc>(entity);
                    if (auth.type is CommandType.PlayerReconnect && auth.sessionId == lobbyData.sessionId)
                    {
                        var trackedPlayers = SystemAPI.GetSingletonBuffer<ServerTrackedPlayer>();
                        for (var i = 0; i < trackedPlayers.Length; i++)
                        {
                            ref var serverTrackedPlayer = ref trackedPlayers.ElementAt(i);
                            if (serverTrackedPlayer.playerId == auth.playerId)
                            {
                                serverTrackedPlayer.networkEntity = SystemAPI
                                    .GetComponent<ReceiveRpcCommandRequest>(entity)
                                    .SourceConnection;
                                ApprovePlayer(serverTrackedPlayer, lobbyData.sessionId, CommandType.ServerReconnect);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var entity in requests)
                {
                    var connection = SystemAPI.GetComponent<ReceiveRpcCommandRequest>(entity);
                    var newGuid = GenerateGuid();
                    var playerId = newGuid.GetHashCode();

                    // Add new authorized player to tracker
                    var trackedPlayers = SystemAPI.GetSingletonBuffer<ServerTrackedPlayer>();
                    var newPlayer = new ServerTrackedPlayer()
                    {
                        playerGuid = newGuid,
                        playerId = playerId,
                        networkEntity = connection.SourceConnection
                    };
                    trackedPlayers.Add(newPlayer);

                    ApprovePlayer(newPlayer, lobbyData.sessionId, CommandType.ServerConnect);
                }
            }

            EntityManager.DestroyEntity(_authorizationQuery);
        }

        private void ApprovePlayer(ServerTrackedPlayer player, int sessionId, CommandType commandType)
        {
            var req = EntityManager.CreateEntity();
            EntityManager.AddComponent(req, _serverSendAuthTypeSet);
            var authorizationRpc = new AuthorizationRpc
            {
                type = commandType,
                playerId = player.playerId,
                sessionId = sessionId
            };
            SystemAPI.SetComponent(req, authorizationRpc);
            SystemAPI.SetComponent(req, new SendRpcCommandRequest
            {
                TargetConnection = player.networkEntity
            });

            Debug.Log(
                $"[{World.Name}] authorized new player: {authorizationRpc.type}, {authorizationRpc.playerId}, {authorizationRpc.sessionId}.");

            // Add authorization cleanup to track disconnection
            EntityManager.AddComponentData(player.networkEntity,
                new AuthorizedPlayer { playerId = player.playerId });
            // Enable syncing on confirmed player
            EntityManager.AddComponent<NetworkStreamInGame>(player.networkEntity);
            // Remove pending to avoid disconnection
            EntityManager.RemoveComponent<PendingApproval>(player.networkEntity);
        }

        private static unsafe Hash128 GenerateGuid()
        {
            var guid = Guid.NewGuid();
            var newGuid = new Hash128();

            var adr = UnsafeUtility.AddressOf(ref newGuid);
            var span = new Span<byte>(adr, sizeof(Hash128));
            if (!guid.TryWriteBytes(span))
            {
                Debug.LogError("failed writing guid");
            }

            return newGuid;
        }
    }
}