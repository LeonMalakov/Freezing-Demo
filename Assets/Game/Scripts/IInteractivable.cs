using UnityEngine;

namespace WGame
{
    public interface IInteractivable
    {
        Vector3 Position { get; }

        bool Interact(Character character);

        bool InteractWithItem(Character character, Item item);

        void BecomeActive();

        void BecomeInactive();
    }
}