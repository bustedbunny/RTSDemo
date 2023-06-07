using System.Runtime.CompilerServices;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Burst;
using Unity.DebugDisplay;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;

namespace TankEntitiesMultiplayer.GamePlay
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct TankTurretRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var delta = SystemAPI.Time.DeltaTime * 1f;

            foreach (var (tankRef, turret, rotRef, tf) in SystemAPI
                         .Query<RefRW<UnitInput>, TankWithTurret, RefRW<TankTurretRotation>, LocalTransform>())
            {
                ref var input = ref tankRef.ValueRW;

                if (!input.hasTarget)
                {
                    continue;
                }

                if (input.enemy != Entity.Null)
                {
                    if (!SystemAPI.HasComponent<LocalTransform>(input.enemy))
                    {
                        input.enemy = Entity.Null;
                        input.hasTarget = false;
                        return;
                    }
                    else
                    {
                        var enemyLt = SystemAPI.GetComponent<LocalTransform>(input.enemy);
                        input.lookPosition = enemyLt.Position;
                    }
                }


                ref var turretRot = ref rotRef.ValueRW;
                ref var turretTf = ref SystemAPI.GetComponentRW<LocalTransform>(turret.turret).ValueRW;
                // turret
                {
                    var worldMatrix = new LocalToWorld { Value = math.mul(tf.ToMatrix(), turretTf.ToMatrix()) };
                    var tankUp = worldMatrix.Up;

                    var tarDir = input.lookPosition - worldMatrix.Position;
                    tarDir = ProjectOnPlane(tarDir, tankUp);
                    tarDir = math.normalize(tarDir);

                    var curDir = worldMatrix.Forward;
                    curDir = ProjectOnPlane(curDir, tankUp);
                    curDir = math.normalize(curDir);

                    var angleBetween = AngleRadians(curDir, tarDir);
                    angleBetween = math.clamp(delta, 0f, angleBetween);

                    var speedMultiplier = math.dot(tarDir, worldMatrix.Right) > 0f ? 1f : -1f;


                    turretRot.turretRotation += speedMultiplier * angleBetween;

                    turretTf.Rotation = quaternion.Euler(0f, turretRot.turretRotation, 0f);
                }
                // Cannon
                {
                    ref var cannonTf = ref SystemAPI.GetComponentRW<LocalTransform>(turret.cannon).ValueRW;
                    var worldMatrix = new LocalToWorld
                        { Value = math.mul(tf.ToMatrix(), math.mul(turretTf.ToMatrix(), cannonTf.ToMatrix())) };

                    var rotationAxis = worldMatrix.Right;

                    var tarDir = input.lookPosition - worldMatrix.Position;
                    tarDir = ProjectOnPlane(tarDir, rotationAxis);
                    tarDir = math.normalize(tarDir);

                    var curDir = worldMatrix.Forward;
                    curDir = ProjectOnPlane(curDir, rotationAxis);
                    curDir = math.normalize(curDir);

                    var angleBetween = AngleRadians(curDir, tarDir);
                    angleBetween = math.clamp(delta, 0f, angleBetween);

                    var speedMultiplier = math.dot(tarDir, -worldMatrix.Up) > 0f ? 1f : -1f;

                    turretRot.cannonRotation += speedMultiplier * angleBetween;
                    turretRot.cannonRotation = math.clamp(turretRot.cannonRotation, turret.cannonConstraints.x,
                        turret.cannonConstraints.y);

                    cannonTf.Rotation = quaternion.Euler(turretRot.cannonRotation, 0f, 0f);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float3 ProjectOnPlane(float3 vector, float3 onPlaneNormal)
        {
            return vector - math.projectsafe(vector, onPlaneNormal);
        }

        private static float AngleRadians(float3 from, float3 to)
        {
            var denominator = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
            if (denominator < math.EPSILON)
                return 0F;

            var dot = math.clamp(math.dot(from, to) / denominator, -1f, 1f);
            return math.acos(dot);
        }
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct ClientTurretDebugRotationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsDebugDisplayData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            SystemAPI.GetSingleton<PhysicsDebugDisplayData>();
            foreach (var (turret, rotRef) in SystemAPI.Query<TankWithTurret, TankTurretRotation>())
            {
                ref var turretTf = ref SystemAPI.GetComponentRW<LocalTransform>(turret.turret).ValueRW;
                turretTf.Rotation = quaternion.RotateY(rotRef.turretRotation);

                ref var cannonTf = ref SystemAPI.GetComponentRW<LocalTransform>(turret.cannon).ValueRW;
                cannonTf.Rotation = quaternion.RotateX(rotRef.cannonRotation);

                var cannonLtw = SystemAPI.GetComponent<LocalToWorld>(turret.cannon);
                var ray = new UnityEngine.Ray(cannonLtw.Position, cannonLtw.Forward);

                PhysicsDebugDisplaySystem.Line(cannonLtw.Position, cannonLtw.Position + cannonLtw.Forward * 100f,
                    ColorIndex.White);
            }
        }
    }
}