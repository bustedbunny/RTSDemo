using BovineLabs.Core.Extensions;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace TankEntitiesMultiplayer.GamePlay
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct TankShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

            var tfLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            var parentLookup = SystemAPI.GetComponentLookup<Parent>(true);
            var scaleLookup = SystemAPI.GetComponentLookup<PostTransformMatrix>(true);

            foreach (var (input, shooter) in SystemAPI.Query<UnitInput, TankShooter>().WithAll<Simulate>())
            {
                if (!input.shoot)
                {
                    continue;
                }

                var shot = ecb.Instantiate(shooter.ammoPrefab);

                TransformHelpers.ComputeWorldTransformMatrix(shooter.shootingPosition, out var shotPosition,
                    ref tfLookup, ref parentLookup, ref scaleLookup);

                var prefabTf = SystemAPI.GetComponent<LocalTransform>(shooter.ammoPrefab);
                prefabTf.Position = shotPosition.Position();
                prefabTf.Rotation = shotPosition.Rotation();
                ecb.SetComponent(shot, prefabTf);

                var shotVelocity = new float3(0f, 0f, shooter.initialVelocity);
                shotVelocity = math.mul(prefabTf.Rotation, shotVelocity);


                ecb.SetComponent(shot, new PhysicsVelocity { Linear = shotVelocity });
            }

            ecb.Playback(state.EntityManager);
        }
    }
}