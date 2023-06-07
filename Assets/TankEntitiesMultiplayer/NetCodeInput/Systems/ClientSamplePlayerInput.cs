using TankEntitiesMultiplayer.Data;
using TankEntitiesMultiplayer.NetCodeInput.Data;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Physics;

namespace TankEntitiesMultiplayer.NetCodeInput.Systems
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct ClientSamplePlayerInput : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var localInput = LocalClientInput.Get;

            var left = localInput.a;
            var right = localInput.d;
            var down = localInput.s;
            var up = localInput.w;

            var ray = localInput.cameraCursorRay;
            var cw = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;


            var mouseoverEntity = Entity.Null;
            if (localInput.select)
            {
                var raycastInput = new RaycastInput
                {
                    Filter = new()
                    {
                        BelongsTo = int.MaxValue,
                        CollidesWith = int.MaxValue,
                        GroupIndex = 0
                    },
                    Start = ray.origin,
                    End = ray.origin + ray.direction * 10000f
                };

                if (cw.CastRay(raycastInput, out var hit))
                {
                    mouseoverEntity = hit.Entity;
                }

                SystemAPI.SetSingleton(new UnitSelection() { entity = mouseoverEntity });
            }

            foreach (var playerInput in SystemAPI.Query<RefRW<PlayerInput>>().WithAll<GhostOwnerIsLocal>())
            {
                ref var input = ref playerInput.ValueRW;

                input.horizontal = default;
                input.vertical = default;
                input.aimOrigin = default;
                input.aimDirection = default;
                input.shoot = default;
                input.order = default;

                input.horizontal = left ? input.horizontal - 1 : input.horizontal;
                input.horizontal = right ? input.horizontal + 1 : input.horizontal;
                input.vertical = down ? input.vertical - 1 : input.vertical;
                input.vertical = up ? input.vertical + 1 : input.vertical;

                input.aimOrigin = ray.origin;
                input.aimDirection = ray.direction;

                if (localInput.action)
                {
                    input.shoot.Set();
                }


                // if (click)
                // {
                //     input.selectedEntity = mouseoverEntity;
                // }
            }
        }
    }
}