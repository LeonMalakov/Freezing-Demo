namespace WGame
{
    public class PlayerHealthView : PlayerStatView
    {
        protected override int GetMaxValue() => Player.MaxHealth;

        protected override int GetCurrentValue() => Player.Health;

        protected override void Subscribe()
        {
            Player.HealthChanged += OnValueChanged;
        }

        protected override void Unsubscribe()
        {
            Player.HealthChanged -= OnValueChanged;
        }
    }
}