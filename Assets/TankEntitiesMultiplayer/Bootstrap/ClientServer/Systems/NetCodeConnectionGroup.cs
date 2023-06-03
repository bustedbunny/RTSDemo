using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ServerSimulation |
                       WorldSystemFilterFlags.ThinClientSimulation)]
    public partial class NetCodeConnectionGroup : ComponentSystemGroup { }
}