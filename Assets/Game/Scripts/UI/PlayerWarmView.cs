namespace WGame
{
    public class PlayerWarmView : PlayerStatView
    {
        protected override int GetMaxValue(Player player) => player.MaxWarm;

        protected override int GetCurrentValue(Player player) => player.Warm;

        protected override void Subscribe(Player player)
        {
            player.WarmChanged += OnValueChanged;
        }

        protected override void Unsubscribe(Player player)
        {
            player.WarmChanged -= OnValueChanged;
        }
    }
}
