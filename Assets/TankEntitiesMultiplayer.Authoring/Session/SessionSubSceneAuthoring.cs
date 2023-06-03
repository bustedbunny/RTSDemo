using TankEntitiesMultiplayer.Data.Session;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring.Session
{
    public class SessionSubSceneAuthoring : MonoBehaviour
    {
        public class SessionSubSceneBaker : Baker<SessionSubSceneAuthoring>
        {
            public override void Bake(SessionSubSceneAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddBuffer<SessionSubScene>(entity);
            }
        }
    }
}