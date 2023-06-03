using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Bootstrap.Testing
{
    [WorldSystemFilter(WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct ThinClientAuthSystem : ISystem
    {
        private EntityQuery _query;

        public void OnCreate(ref SystemState state)
        {
            _query = SystemAPI.QueryBuilder().WithAll<NetworkId>().WithNone<AuthorizeConnectionAsPlayer>().Build();
            state.RequireForUpdate(_query);
        }

        public void OnUpdate(ref SystemState state)
        {
            state.EntityManager.AddComponent<AuthorizeConnectionAsPlayer>(_query);
        }
    }
}