using TankEntitiesMultiplayer.Data;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(NetCodeConnectionGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct ClientInitializePlayerInputSystem : ISystem
    {
        private EntityQuery _initQuery;
        private EntityQuery _toInitQuery;

        public void OnCreate(ref SystemState state)
        {
            _toInitQuery = SystemAPI.QueryBuilder().WithAll<PlayerInput, GhostOwnerIsLocal>()
                .WithNone<InitializedClientInput>().Build();
            state.RequireForUpdate(_toInitQuery);
        }

        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.HasSingleton<InitializedClientInput>())
            {
                return;
            }

            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            var count = 0;

            foreach (var (input, e) in SystemAPI.Query<PlayerInput>().WithAll<GhostOwnerIsLocal>().WithEntityAccess())
            {
                ecb.AddComponent<InitializedClientInput>(e);
                count++;
            }

            if (count != 1)
            {
                Debug.LogError("More than one locally owned input initialized.");
            }

            ecb.Playback(state.EntityManager);
        }
    }
}