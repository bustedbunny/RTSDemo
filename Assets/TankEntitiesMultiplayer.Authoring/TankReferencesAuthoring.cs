using TankEntitiesMultiplayer.Data;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring
{
    public class TankReferencesAuthoring : MonoBehaviour
    {
        public GameObject t34;

        public class TankReferencesAuthoringBaker : Baker<TankReferencesAuthoring>
        {
            public override void Bake(TankReferencesAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new TankReferences
                {
                    t34 = GetEntity(authoring.t34, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}