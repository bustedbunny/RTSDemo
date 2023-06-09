﻿using Pathfinding.Components;
using TankEntitiesMultiplayer.Bootstrap;
using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Tank;
using TankEntitiesMultiplayer.NetCodeInput.Data;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;

namespace TankEntitiesMultiplayer.NetCodeInput.Systems
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct ClientOrderUnitMovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InitializedClientInput>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var localInput = LocalClientInput.Get;

            if (!localInput.action)
            {
                return;
            }

            var selection = SystemAPI.GetSingleton<UnitSelection>();

            var localInputEntity = SystemAPI.GetSingletonEntity<InitializedClientInput>();
            ref var input = ref SystemAPI.GetComponentRW<PlayerInput>(localInputEntity).ValueRW;

            if (selection.entity != Entity.Null && SystemAPI.HasComponent<Pathfinder>(selection.entity))
            {
                var pws = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
                var ray = localInput.cameraCursorRay;
                var raycastInput = new RaycastInput
                {
                    Filter = new()
                    {
                        BelongsTo = int.MaxValue,
                        CollidesWith = int.MaxValue,
                        GroupIndex = 0
                    },
                    Start = ray.origin,
                    End = ray.origin + ray.direction * 10000f
                };

                if (pws.CastRay(raycastInput, out var hit))
                {
                    // order to attack enemy
                    if (SystemAPI.HasComponent<ControllableUnit>(hit.Entity))
                    {
                        input.orderType = Order.Attack;
                        input.controlledUnit = selection.entity;
                        input.unitToAttack = hit.Entity;
                        input.order.Set();
                    }
                    else
                    {
                        input.orderType = Order.Move;
                        var lt = SystemAPI.GetComponent<LocalTransform>(selection.entity);
                        input.from = lt.Position;
                        input.to = hit.Position;
                        input.order.Set();
                        input.controlledUnit = selection.entity;
                        SystemAPI.GetComponentRW<Pathfinder>(selection.entity).ValueRW.to = hit.Position;
                    }
                }
            }
        }
    }
}