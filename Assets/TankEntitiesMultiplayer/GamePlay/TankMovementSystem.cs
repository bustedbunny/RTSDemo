using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace TankEntitiesMultiplayer.GamePlay
{
    [UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct TankMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var speed = SystemAPI.Time.DeltaTime * 4;

            foreach (var (tank, trans, physicsVelocity) in SystemAPI
                         .Query<UnitInput, RefRO<LocalTransform>, RefRW<PhysicsVelocity>>()
                         .WithAll<Simulate>())
            {
                ref var pv = ref physicsVelocity.ValueRW;
                ref readonly var tf = ref trans.ValueRO;
                ref readonly var inp = ref tank;

                var moveInput = new float2(inp.horizontal, inp.vertical) * speed;
                var direction = math.sign(inp.vertical);
                direction = math.select(direction, 1f, direction == 0f);

                pv.Linear += tf.TransformDirection(new(0f, 0f, moveInput.y * 3f));
                pv.Angular.y += moveInput.x * direction;
            }
        }
    }
}