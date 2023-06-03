using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace TankEntitiesMultiplayer.NetCodeInput.Data
{
    public struct LocalClientInput
    {
        public bool a, d, s, w;

        public bool lmbDown;
        public bool rmbDown, mmbDown, mmbPress;

        public Ray cameraCursorRay;

        public static ref LocalClientInput Get => ref Input.Data;

        private static readonly SharedStatic<LocalClientInput> Input =
            SharedStatic<LocalClientInput>.GetOrCreate<LocalClientInput, InputFieldKey>();

        // ReSharper disable once ClassNeverInstantiated.Local
        private class InputFieldKey { }
    }

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SampleLocalClientInputSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // Init shared static
            _ = LocalClientInput.Get;
        }

        public void OnUpdate(ref SystemState state)
        {
            ref var input = ref LocalClientInput.Get;

            input.a = Input.GetKey(KeyCode.A);
            input.d = Input.GetKey(KeyCode.D);
            input.s = Input.GetKey(KeyCode.S);
            input.w = Input.GetKey(KeyCode.W);

            input.lmbDown = Input.GetMouseButtonDown(0);
            input.rmbDown = Input.GetMouseButtonDown(1);
            input.mmbDown = Input.GetMouseButtonDown(2);
            input.mmbPress = Input.GetMouseButton(2);

            var camera = Camera.main;
            input.cameraCursorRay = camera != null ? camera.ScreenPointToRay(Input.mousePosition) : default;
        }
    }
}