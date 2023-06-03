using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace TankEntitiesMultiplayer.Data
{
    [GhostComponent(PrefabType = GhostPrefabType.All)]
    public struct PlayerInput : IInputComponentData
    {
        public int horizontal;
        public int vertical;

        [GhostField(Quantization = 1000)] public float3 aimOrigin;
        [GhostField(Quantization = 1000)] public float3 aimDirection;

        public InputEvent shoot;

        [GhostField] public Entity selectedEntity;


        // AI unit moving
        public InputEvent moveUnit;
        [GhostField(Quantization = 1000)] public float3 from, to;
        [GhostField] public Entity unitToMove;
    }

    [GhostComponent]
    public struct PlayerId : IComponentData
    {
        public int playerId;
    }
}