using UnityEngine;

namespace WGame
{
    public abstract class Interactivable : MonoBehaviour
    {
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