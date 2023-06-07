using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TankEntitiesMultiplayer.NetCodeInput.Data
{
    public struct LocalClientInput
    {
        public bool a, d, s, w;

        public bool select;
        public bool action;

        public Ray cameraCursorRay;


        public bool dragStarted;
        public bool draggingCamera;

        public float2 dragDelta;

        public static ref LocalClientInput Get => ref Input.Data;

        private static readonly SharedStatic<LocalClientInput> Input =
            SharedStatic<LocalClientInput>.GetOrCreate<LocalClientInput, InputFieldKey>();

        // ReSharper disable once ClassNeverInstantiated.Local
        private class InputFieldKey { }
    }

    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class SampleLocalClientInputSystem : SystemBase
    {
        private PlayerControls.PlayerControls _controls;

        protected override void OnCreate()
        {
            _controls = new();
            _controls.Enable();
            // Init shared static
            _ = LocalClientInput.Get;
        }

        protected override void OnDestroy()
        {
            _controls.Dispose();
        }

        protected override void OnUpdate()
        {
            ref var input = ref LocalClientInput.Get;

            var dragging = _controls.CameraControls.Drag.IsPressed();
            input.dragStarted = !input.draggingCamera && dragging;
            input.draggingCamera = dragging;

            input.dragDelta = _controls.CameraControls.DragDelta.ReadValue<Vector2>();

            input.a = Input.GetKey(KeyCode.A);
            input.d = Input.GetKey(KeyCode.D);
            input.s = Input.GetKey(KeyCode.S);
            input.w = Input.GetKey(KeyCode.W);

            input.select = _controls.CameraControls.Select.WasPerformedThisFrame();
            input.action = _controls.CameraControls.Action.WasPerformedThisFrame();

            var camera = Camera.main;
            input.cameraCursorRay = camera != null ? camera.ScreenPointToRay(Input.mousePosition) : default;
        }
    }
}