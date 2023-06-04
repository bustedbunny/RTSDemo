using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace TankEntitiesMultiplayer.NetCodeInput.Selection
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct ClientMoveSelectionDrawingSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CoreReferences>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            var references = SystemAPI.GetSingleton<CoreReferences>();
            state.EntityManager.Instantiate(references.selectionPrefab);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var selectionEntity = SystemAPI.GetSingletonEntity<UnitSelection>();
            var selection = SystemAPI.GetSingleton<UnitSelection>().entity;
            ref var lt = ref SystemAPI.GetComponentRW<LocalTransform>(selectionEntity).ValueRW;
            if (selection == Entity.Null || !SystemAPI.HasComponent<UnitInput>(selection))
            {
                lt.Scale = 0;
            }
            else
            {
                if (SystemAPI.HasComponent<WorldRenderBounds>(selection))
                {
                    var worldBounds = SystemAPI.GetComponent<WorldRenderBounds>(selection);
                    lt.Scale = math.max(worldBounds.Value.Size.x, worldBounds.Value.Size.z);
                    lt.Position = worldBounds.Value.Center;
                }
                else
                {
                    var selectionLt = SystemAPI.GetComponent<LocalTransform>(selection);
                    lt.Position = selectionLt.Position;
                    lt.Scale = 2f;
                }
            }
        }


        public void OnStopRunning(ref SystemState state) { }
    }
}