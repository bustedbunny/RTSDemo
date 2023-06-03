using TankEntitiesMultiplayer.NetCodeInput.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TankEntitiesMultiplayer.NetCodeCamera
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial class CameraDragSystem : SystemBase
    {
        private Vector3 _origin;
        private float _dragSpeed = 15f;
        private Vector3 _originPosition;

        protected override void OnUpdate()
        {
            var input = LocalClientInput.Get;


            var playerTransform = PlayerTransform.Instance.transform;
            if (input.mmbDown)
            {
                _origin = Input.mousePosition;
                _originPosition = playerTransform.position;
                return;
            }

            if (!input.mmbPress) return;

            var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _origin);
            var move = new Vector3(pos.x * _dragSpeed, 0f, pos.y * _dragSpeed) *
                       math.max(1f, playerTransform.position.y / 10f);
            var position = _originPosition - move;
            playerTransform.position = position;
        }
    }

    public partial class CameraZoomSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel") * 15f;
            PlayerTransform.Instance.transform.Translate(0f, -scroll, 0f);
        }
    }
}