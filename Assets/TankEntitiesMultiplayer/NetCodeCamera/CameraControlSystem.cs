using TankEntitiesMultiplayer.NetCodeInput.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace TankEntitiesMultiplayer.NetCodeCamera
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial class CameraControlSystem : SystemBase
    {
        private float _dragSpeed = 10f;
        private Vector3 _originPosition;

        private Camera _mainCamera;
        private Transform _playerTransform;

        protected override void OnStartRunning()
        {
            _mainCamera = Camera.main;
            _playerTransform = PlayerTransform.Instance.transform;
        }

        protected override void OnUpdate()
        {
            var input = LocalClientInput.Get;
            HandleDrag(input);
            HandleZoom();
        }

        private void HandleDrag(LocalClientInput input)
        {
            var playerTransform = PlayerTransform.Instance.transform;

            if (input.dragStarted)
            {
                _originPosition = playerTransform.position;
                return;
            }

            if (!input.draggingCamera) return;

            var curPosition = playerTransform.position;

            var pos = _mainCamera.ScreenToViewportPoint((Vector2)input.dragDelta);
            var move = -_dragSpeed * math.max(3f, curPosition.y / 5f) * new Vector3(pos.x, 0f, pos.y);
            playerTransform.Translate(move);
        }

        private void HandleZoom()
        {
            var scroll = Input.mouseScrollDelta.y * 5f;

            if (scroll == 0f) return;


            float3 playerPos = _playerTransform.localPosition;
            var cameraPos = playerPos + new float3(0, 15f, -10f);

            var cameraDelta = cameraPos - playerPos;

            var cameraDirection = math.normalize(cameraDelta);
            var targetPosition = playerPos - cameraDirection * scroll;

            if (scroll > 0)
            {
                var input = new RaycastInput
                {
                    Start = cameraPos,
                    End = targetPosition + cameraDelta,
                    Filter = CollisionFilter.Default
                };


                if (SystemAPI.GetSingleton<PhysicsWorldSingleton>().CastRay(input, out var hit))
                {
                    Debug.Log(hit.Entity);
                    targetPosition = hit.Position - cameraDelta + cameraDirection * 2f;
                }
            }

            _playerTransform.localPosition = targetPosition;
        }
    }
}