using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    public struct LoadedSubScenes : IBufferElementData
    {
        public Entity scene;
        public Hash128 hash;

        public bool isLoaded;
    }
}