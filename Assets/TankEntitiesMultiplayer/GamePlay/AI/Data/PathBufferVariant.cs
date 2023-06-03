using System;
using Pathfinding.Components;
using Unity.Burst;
using Unity.DebugDisplay;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics.Authoring;
using Unity.Transforms;

namespace TankEntitiesMultiplayer.GamePlay.AI.Data
{
    // [GhostComponentVariation(typeof(PathBuffer), "Path Buffer Full")]
    // public struct PathBufferVariant : IBufferElementData
    // {
    //     [GhostField(Quantization = 1000)] public float3 position;
    // }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct PathDrawSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsDebugDisplayData>();
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            SystemAPI.GetSingleton<PhysicsDebugDisplayData>();
            foreach (var (pathfinder, lt) in SystemAPI.Query<Pathfinder, LocalTransform>()
                         .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                PhysicsDebugDisplaySystem.Line(lt.Position, pathfinder.to, ColorIndex.Red);
            }
        }
    }
}