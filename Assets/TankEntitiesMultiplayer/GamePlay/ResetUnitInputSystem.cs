using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.Entities;

namespace TankEntitiesMultiplayer.GamePlay
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial struct ResetUnitInputSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var unitInputRef in SystemAPI.Query<RefRW<UnitInput>>())
            {
                ref var input = ref unitInputRef.ValueRW;
                input.horizontal = 0;
                input.vertical = 0;
            }
        }
    }
}