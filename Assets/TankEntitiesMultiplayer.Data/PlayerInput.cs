using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine.Serialization;

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


        // AI unit controls
        public InputEvent order;
        [GhostField] public Order orderType;
        [GhostField(Quantization = 1000)] public float3 from, to;
        [GhostField] public Entity controlledUnit;
        [GhostField] public Entity unitToAttack;
    }

    public enum Order
    {
        Move,
        Attack
    }

    [GhostComponent]
    public struct PlayerId : IComponentData
    {
        public int playerId;
    }
}