namespace WGame
{
    public interface IInteractivable : IGameObject
    {
        bool Interact(Player character);

        bool InteractWithItem(Player character, Item item);

        void BecomeActive();

        void BecomeInactive();
    }
}