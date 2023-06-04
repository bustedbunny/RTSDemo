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
        private Vector3 _origin;
        private float _dragSpeed = 15f;
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
            if (input.mmbDown)
            {
                _origin = Input.mousePosition;
                _originPosition = playerTransform.position;
                return;
            }

            if (!input.mmbPress) return;

            var pos = _mainCamera.ScreenToViewportPoint(Input.mousePosition - _origin);
            var move = new Vector3(pos.x * _dragSpeed, 0f, pos.y * _dragSpeed) *
                       math.max(1f, playerTransform.position.y / 10f);
            var position = _originPosition - move;
            playerTransform.position = position;
        }

        private void HandleZoom()
        {
            var scroll = Input.mouseScrollDelta.y * 5f;

            if (scroll == 0f) return;


            float3 playerPos = _playerTransform.localPosition;
            float3 cameraPos = _mainCamera.transform.position;

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