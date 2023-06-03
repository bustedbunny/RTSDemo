using UnityEngine;

namespace TankEntitiesMultiplayer.NetCodeCamera
{
    public class PlayerTransform : MonoBehaviour
    {
        private void Awake()
        {
            Instance = gameObject;
        }

        public static GameObject Instance { get; private set; }
    }
}