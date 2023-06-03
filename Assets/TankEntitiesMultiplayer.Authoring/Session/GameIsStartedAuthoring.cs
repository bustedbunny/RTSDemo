using TankEntitiesMultiplayer.Data.Session;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.Authoring.Session
{
    public class GameIsStartedAuthoring : MonoBehaviour
    {
        public class GameIsStartedBaker : Baker<GameIsStartedAuthoring>
        {
            public override void Bake(GameIsStartedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new GameIsStarted());
            }
        }
    }
}