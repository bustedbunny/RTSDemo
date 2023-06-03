using System.Runtime.CompilerServices;
using Pathfinding.Components;
using TankEntitiesMultiplayer.Data.Tank;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TankEntitiesMultiplayer.GamePlay.AI
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct TankAIMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (inputRef, path, lt) in SystemAPI
                         .Query<RefRW<UnitInput>, DynamicBuffer<PathBuffer>, LocalTransform>())
            {
                if (path.IsEmpty)
                {
                    continue;
                }

                var currentTarget = path[0].position;

                var delta = currentTarget - lt.Position;
                if (math.distancesq(currentTarget, lt.Position) <= 2.5f)
                {
                    path.RemoveAt(0);
                    return;
                }

                var curForward = lt.Forward();
                curForward = ProjectOnPlane(curForward, math.up());
                curForward = math.normalize(curForward);
                var tarForward = math.normalize(delta);
                tarForward = ProjectOnPlane(tarForward, math.up());
                tarForward = math.normalize(tarForward);

                var differenceDot = math.dot(curForward, tarForward);
                var shouldMove = differenceDot >= 0.98f;

                if (shouldMove)
                {
                    // Move
                    inputRef.ValueRW.vertical = 1;
                }
                else
                {
                    // Rotate
                    var directionDot = math.dot(tarForward, lt.Right());
                    var movement = directionDot < 0 ? -1 : 1;
                    inputRef.ValueRW.horizontal = movement;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float3 ProjectOnPlane(float3 vector, float3 onPlaneNormal)
        {
            return vector - math.projectsafe(vector, onPlaneNormal);
        }
    }
}