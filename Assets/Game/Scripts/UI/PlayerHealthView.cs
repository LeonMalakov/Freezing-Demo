namespace WGame
{
    public class PlayerHealthView : PlayerStatView
    {
        protected override int GetMaxValue(Player player) => player.MaxHealth;

        protected override int GetCurrentValue(Player player) => player.Health;

        protected override void Subscribe(Player player)
        {
            player.HealthChanged += OnValueChanged;
        }

        protected override void Unsubscribe(Player player)
        {
            player.HealthChanged -= OnValueChanged;
        }
    }
}