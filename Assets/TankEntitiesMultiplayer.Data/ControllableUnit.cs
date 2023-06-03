using Unity.Entities;

namespace TankEntitiesMultiplayer.Data
{
    public struct ControllableUnit : IComponentData
    {
        public int playedId;
    }
}