using UnityEngine;

namespace WGame
{
    public class GameBehaviour : MonoBehaviour
    {
        public EarthPlacer EarthPlacer { get; private set; }

        protected virtual void Awake()
        {
            EarthPlacer = GetComponent<EarthPlacer>();
        }
    }
}
