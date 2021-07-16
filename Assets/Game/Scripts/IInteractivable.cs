using UnityEngine;

namespace WGame
{
    public interface IInteractivable
    {
        Vector3 Position { get; }

        bool Interact(Player character);

        bool InteractWithItem(Player character, Item item);

        void BecomeActive();

        void BecomeInactive();
    }
}