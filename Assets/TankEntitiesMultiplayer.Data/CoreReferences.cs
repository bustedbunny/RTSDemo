using Unity.Entities;

namespace TankEntitiesMultiplayer.Data
{
    public struct CoreReferences : IComponentData
    {
        public Entity runtimePrefab;
        public Entity connectionPrefab;

        public Entity loadingPrefab;

        public Entity gameIsStartedPrefab;

        public Entity selectionPrefab;

        public Hash128 gameSubScene;
    }
}