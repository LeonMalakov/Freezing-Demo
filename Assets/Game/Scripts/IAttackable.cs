namespace WGame
{
    public interface IAttackable : IGameObject
    {
        bool IsPriority { get; }

        bool IsAlive { get; }

        void TakeDamage(int damage);
    }
}
