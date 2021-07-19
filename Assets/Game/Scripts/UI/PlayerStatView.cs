using UnityEngine;
using WGame.UI;

namespace WGame
{
    [RequireComponent(typeof(ProgressBar))]
    public abstract class PlayerStatView : MonoBehaviour, IPlayerTargeting
    {
        private ProgressBar _bar;

        protected Player Player { get; private set; }

        public void SetTarget(Player player)
        {
            TryUnsubscribe();
            Player = player;

            if (isActiveAndEnabled)
                TrySubscribe();
        }

        private void Awake()
        {
            _bar = GetComponent<ProgressBar>();
        }

        private void OnEnable()
        {
            TrySubscribe();
        }

        private void OnDisable()
        {
            TryUnsubscribe();
        }

        private void TrySubscribe()
        {
            if (Player != null)
            {
                Subscribe();
                OnValueChanged(GetCurrentValue());
            }
        }

        private void TryUnsubscribe()
        {
            if (Player != null)
                Unsubscribe();
        }


        protected void OnValueChanged(int value)
        {
            _bar.SetProgress(value / (float)GetMaxValue());
        }

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();

        protected abstract int GetMaxValue();

        protected abstract int GetCurrentValue();
    }
}