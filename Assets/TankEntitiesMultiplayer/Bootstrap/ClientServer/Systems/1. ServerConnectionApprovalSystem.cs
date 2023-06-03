using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial class ServerConnectionApprovalSystem : SystemBase
    {
        private EntityQuery _connectionQuery;
        private EntityQuery _pendingQuery;
        private ComponentTypeSet _typeSet;

        private struct Init : IComponentData { }

        protected override void OnCreate()
        {
            _typeSet = ComponentTypeSetUtility.Create<PendingApproval, Init>();
            _connectionQuery = SystemAPI.QueryBuilder().WithAll<NetworkId>().WithNone<PendingApproval, Init>().Build();
            _pendingQuery = SystemAPI.QueryBuilder().WithAll<NetworkId, PendingApproval, Init>().Build();

            RequireAnyForUpdate(_connectionQuery, _pendingQuery);
        }

        protected override void OnUpdate()
        {
            if (!_pendingQuery.IsEmptyIgnoreFilter)
            {
                var curTime = SystemAPI.Time.ElapsedTime;

                var pending = _pendingQuery.ToEntityArray(WorldUpdateAllocator);
                foreach (var entity in pending)
                {
                    var time = SystemAPI.GetComponent<PendingApproval>(entity).kickTime;
                    if (curTime >= time)
                    {
                        Debug.Log($"[{World.Name}] disconnecting connection {entity}");
                        EntityManager.AddComponent<NetworkStreamRequestDisconnect>(entity);
                    }
                }
            }

            if (!_connectionQuery.IsEmptyIgnoreFilter)
            {
                var kickTime = SystemAPI.Time.ElapsedTime + 5;
                var connected = _connectionQuery.ToEntityArray(WorldUpdateAllocator);

                EntityManager.AddComponent(_connectionQuery, _typeSet);

                foreach (var entity in connected)
                {
                    SystemAPI.SetComponent(entity, new PendingApproval { kickTime = kickTime });
                }
            }
        }
    }
}