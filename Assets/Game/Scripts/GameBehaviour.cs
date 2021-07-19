using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(EarthPlacer))]
    public class GameBehaviour : MonoBehaviour
    {
        private EarthPlacer _earthPlacer;
        public EarthPlacer EarthPlacer
        {
            get
            {
                if (_earthPlacer == null)
                    _earthPlacer = GetComponent<EarthPlacer>();

                return _earthPlacer;
            }
        }
    }
}
