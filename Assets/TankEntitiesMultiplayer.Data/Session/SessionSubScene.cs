using Unity.Entities;
using Unity.NetCode;
using Hash128 = Unity.Entities.Hash128;

namespace TankEntitiesMultiplayer.Data.Session
{
    [GhostComponent]
    public struct SessionSubScene : IBufferElementData
    {
        [GhostField] public Hash128 subScene;
    }
}