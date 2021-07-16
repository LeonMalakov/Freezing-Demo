using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(EarthPlacer))]
    public class GameBehaviour : MonoBehaviour
    {
        public EarthPlacer EarthPlacer { get; private set; }

        protected virtual void Awake()
        {
            EarthPlacer = GetComponent<EarthPlacer>();
        }
    }
}
