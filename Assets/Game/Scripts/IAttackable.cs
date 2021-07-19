namespace WGame
{
    public interface IAttackable : IGameObject
    {
        bool IsAlive { get; }

        void TakeDamage(int damage);
    }
}
