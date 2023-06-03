using Pathfinding.Aspects;
using Pathfinding.Components;
using TankEntitiesMultiplayer.Data;
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
                if (!playerInput.moveUnit.IsSet)
                {
                    continue;
                }

                if (!SystemAPI.HasComponent<Pathfinder>(playerInput.unitToMove))
                {
                    continue;
                }

                var unit = SystemAPI.GetComponent<ControllableUnit>(playerInput.unitToMove);
                if (unit.playedId != id.playerId)
                {
                    continue;
                }

                var path = SystemAPI.GetAspect<PathfinderAspect>(playerInput.unitToMove);
                path.FindPath(playerInput.from, playerInput.to);
            }
        }
    }
}