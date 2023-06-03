using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.Entities;

namespace TankEntitiesMultiplayer.GamePlay
{
    public partial struct ResetUnitInputSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var unitInputRef in SystemAPI.Query<RefRW<UnitInput>>())
            {
                unitInputRef.ValueRW = default;
            }
        }
    }
}