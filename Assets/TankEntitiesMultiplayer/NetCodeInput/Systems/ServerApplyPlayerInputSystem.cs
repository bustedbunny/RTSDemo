using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.Entities;

namespace TankEntitiesMultiplayer.NetCodeInput.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerApplyPlayerInputSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerInput, id) in SystemAPI.Query<PlayerInput, PlayerId>().WithChangeFilter<PlayerInput>())
            {
                if (playerInput.selectedEntity == Entity.Null)
                {
                    continue;
                }

                if (SystemAPI.HasComponent<ControllableUnit>(playerInput.selectedEntity))
                {
                    var unit = SystemAPI.GetComponent<ControllableUnit>(playerInput.selectedEntity);
                    if (unit.playedId != id.playerId)
                    {
                        continue;
                    }
                }

                if (SystemAPI.HasComponent<UnitInput>(playerInput.selectedEntity))
                {
                    ref var tankInput = ref SystemAPI.GetComponentRW<UnitInput>(playerInput.selectedEntity).ValueRW;
                    // tankInput.aimDirection = playerInput.aimDirection;
                    // tankInput.aimOrigin = playerInput.aimOrigin;
                    tankInput.horizontal = playerInput.horizontal;
                    tankInput.vertical = playerInput.vertical;
                    tankInput.shoot = playerInput.shoot.IsSet;
                }
            }
        }
    }
}