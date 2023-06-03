using Unity.Entities;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Data
{
    [GhostComponent]
    [InternalBufferCapacity(32)]
    public struct PublicPlayerData : IBufferElementData
    {
        [GhostField] public int playerId;
        [GhostField] public bool isLoaded;
    }
}