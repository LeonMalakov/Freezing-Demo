namespace WGame
{
    public interface IAttackable
    {
        bool IsAlive { get; }

        void TakeDamage(int damage);
    }
}
