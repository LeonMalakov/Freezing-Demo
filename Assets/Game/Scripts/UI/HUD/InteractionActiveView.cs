namespace WGame
{
    public class InteractionActiveView : TargetPointerView
    {
        protected override void Subscribe()
        {
            Player.InteractionActiveChanged += OnActiveChanged;
        }

        protected override void Unsubscribe()
        {
            Player.InteractionActiveChanged -= OnActiveChanged;
        }

        protected override bool UpdatePointerWhile(float t) => true;
    }
}
