using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(EarthPlacer))]
    public abstract class Interactivable : MonoBehaviour
    {
        public EarthPlacer EarthPlacer { get; private set; }

        protected virtual void Awake()
        {
            EarthPlacer = GetComponent<EarthPlacer>();
        }

        public abstract bool Interact(Character character);

        public abstract bool InteractWithItem(Character character, Item item);

        public virtual void BecomeActive()
        {
        }

        public virtual void BecomeInactive()
        {
        }
    }
}