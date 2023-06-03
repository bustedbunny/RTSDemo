using System.Collections;

namespace TankEntitiesMultiplayer
{
    public struct RoutineUpdater
    {
        private IEnumerator _enumerator;

        public RoutineUpdater(IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        public void Update()
        {
            if (_enumerator is null) return;
            if (!_enumerator.MoveNext())
            {
                _enumerator = null;
            }
        }
    }
}