using Unity.Entities;

namespace TankEntitiesMultiplayer
{
    public static class ComponentTypeSetUtility
    {
        public static ComponentTypeSet Create<T0, T1>()
        {
            return new(ComponentType.ReadOnly<T0>(), ComponentType.ReadOnly<T1>());
        }

        public static ComponentTypeSet Create<T0, T1, T2>()
        {
            return new(ComponentType.ReadOnly<T0>(), ComponentType.ReadOnly<T1>(), ComponentType.ReadOnly<T2>());
        }

        public static ComponentTypeSet Create<T0, T1, T2, T3>()
        {
            return new(ComponentType.ReadOnly<T0>(), ComponentType.ReadOnly<T1>(), ComponentType.ReadOnly<T2>(),
                ComponentType.ReadOnly<T3>());
        }

        public static ComponentTypeSet Create<T0, T1, T2, T3, T4>()
        {
            return new(ComponentType.ReadOnly<T0>(), ComponentType.ReadOnly<T1>(), ComponentType.ReadOnly<T2>(),
                ComponentType.ReadOnly<T3>(), ComponentType.ReadOnly<T4>());
        }
    }
}