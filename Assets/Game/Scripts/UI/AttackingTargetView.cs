using UnityEngine;

namespace WGame
{
    public class AttackingTargetView : TargetPointerView
    {
        [SerializeField] private float _disappearTime = 1f;

        protected override void Subscribe()
        {
            Player.Attacking += OnActiveChanged;
        }

        protected override void Unsubscribe()
        {
            Player.Attacking -= OnActiveChanged;
        }

        protected override bool UpdatePointerWhile(float t) => t < _disappearTime;
    }
}
