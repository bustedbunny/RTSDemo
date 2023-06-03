using TankEntitiesMultiplayer.Data;
using Unity.Entities;
using Unity.Scenes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace TankEntitiesMultiplayer.Authoring
{
    public class CoreReferenceAuthoring : MonoBehaviour
    {
        public GameObject playerConnectionPrefab;
        public GameObject runtimePlayerPrefab;

        public GameObject gameIsStartedPrefab;


        public GameObject loadingPrefab;

        public GameObject selectionPrefab;

        public SceneAsset gameSubScene;

        public class PlayerReferenceBaker : Baker<CoreReferenceAuthoring>
        {
            public override void Bake(CoreReferenceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity,
                    new CoreReferences
                    {
                        runtimePrefab = GetEntity(authoring.runtimePlayerPrefab, TransformUsageFlags.None),
                        connectionPrefab = GetEntity(authoring.playerConnectionPrefab, TransformUsageFlags.None),
                        loadingPrefab = GetEntity(authoring.loadingPrefab, TransformUsageFlags.None),
                        gameIsStartedPrefab = GetEntity(authoring.gameIsStartedPrefab, TransformUsageFlags.None),
                        selectionPrefab = GetEntity(authoring.selectionPrefab, TransformUsageFlags.None),
                        gameSubScene =
                            AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(authoring.gameSubScene))
                    });
            }
        }
    }
}