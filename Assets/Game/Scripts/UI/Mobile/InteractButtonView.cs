using UnityEngine;

namespace WGame.Mobile
{
    public class InteractButtonView : MonoBehaviour, IPlayerTargeting
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private Player _player;
        private bool _hasActive;
        private bool _isLoaded;

        public void SetTarget(Player player)
        {
            TryUnsubscribe();
            _player = player;

            if (isActiveAndEnabled)
                TrySubscribe();
        }

        private void OnEnable()
        {
            DisableArea();
            TrySubscribe();
        }

        private void OnDisable()
        {
            TryUnsubscribe();
        }

        private void UpdateAreaState()
        {
            if (_hasActive || _isLoaded)
                EnableArea();
            else
                DisableArea();
        }

        private void EnableArea()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        private void DisableArea()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void TrySubscribe()
        {
            if (_player != null)
                Subscribe();
        }

        private void TryUnsubscribe()
        {
            if (_player != null)
                Unsubscribe();
        }

        private void Subscribe()
        {
            _player.InteractionActiveChanged += OnActiveChanged;
            _player.IsLoadedChanged += OnIsLoadedChanged;
        }

        private void Unsubscribe()
        {
            _player.InteractionActiveChanged -= OnActiveChanged;
            _player.IsLoadedChanged -= OnIsLoadedChanged;
        }

        protected void OnActiveChanged(IGameObject target)
        {
            _hasActive = target != null;
            UpdateAreaState();
        }

        private void OnIsLoadedChanged(bool isLoaded)
        {
            _isLoaded = isLoaded;
            UpdateAreaState();
        }
    }
}