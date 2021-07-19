namespace WGame
{
    public class PlayerWarmView : PlayerStatView
    {
        protected override int GetMaxValue() => Player.MaxWarm;

        protected override int GetCurrentValue() => Player.Warm;

        protected override void Subscribe()
        {
            Player.WarmChanged += OnValueChanged;
        }

        protected override void Unsubscribe()
        {
            Player.WarmChanged -= OnValueChanged;
        }
    }
}
