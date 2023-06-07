using Pathfinding.Aspects;
using Pathfinding.Components;
using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.Entities;

namespace TankEntitiesMultiplayer.NetCodeInput.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerApplyPlayerUnitCommandSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerInput, id) in SystemAPI.Query<PlayerInput, PlayerId>())
            {
                if (!playerInput.order.IsSet)
                {
                    continue;
                }

                // Check if unit can be controlled
                var unit = SystemAPI.GetComponent<ControllableUnit>(playerInput.controlledUnit);
                if (unit.playedId != id.playerId)
                {
                    continue;
                }

                if (playerInput.orderType is Order.Attack)
                {
                    if (!SystemAPI.HasComponent<ControllableUnit>(playerInput.unitToAttack))
                    {
                        continue;
                    }

                    if (!SystemAPI.HasComponent<UnitInput>(playerInput.controlledUnit))
                    {
                        continue;
                    }

                    ref var unitInput = ref SystemAPI.GetComponentRW<UnitInput>(playerInput.controlledUnit).ValueRW;
                    unitInput.hasTarget = true;
                    unitInput.enemy = playerInput.unitToAttack;
                }

                if (playerInput.orderType is Order.Move)
                {
                    if (!SystemAPI.HasComponent<Pathfinder>(playerInput.controlledUnit))
                    {
                        continue;
                    }

                    var path = SystemAPI.GetAspect<PathfinderAspect>(playerInput.controlledUnit);
                    path.FindPath(playerInput.from, playerInput.to);
                }
            }
        }
    }
}