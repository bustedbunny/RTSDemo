using TankEntitiesMultiplayer.Data;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring
{
    public class UnitSelectionAuthoring : MonoBehaviour
    {
        public class UnitSelectionBaker : Baker<UnitSelectionAuthoring>
        {
            public override void Bake(UnitSelectionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitSelection());
            }
        }
    }
}