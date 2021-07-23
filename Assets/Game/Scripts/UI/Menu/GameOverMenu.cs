using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameOverMenu : MonoBehaviour
    {
        private const float ShowAnimationTime = 0.5f;
        private const float HideAnimationTime = 0.5f;

        [SerializeField] private RectTransform _panel;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _restartButton;

        private CanvasGroup _canvasGroup;
        private Coroutine _animationLoop;
        private Action _exit;
        private Action _restart;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            HideCanvasGroup();
        }

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        public void Show(Action exit, Action restart)
        {
            _exit = exit;
            _restart = restart;

            StartShowAnimation();
            ShowCanvasGroup();
        }

        public void Hide()
        {
            StartHideAnimation(callback: HideCanvasGroup);
        }

        private void OnExitButtonClicked()
        {
            StartHideAnimation(callback: () =>
            {
                HideCanvasGroup();
                _exit?.Invoke();
            });
        }

        private void OnRestartButtonClicked()
        {
            StartHideAnimation(callback: () =>
            {
                HideCanvasGroup();
                _restart?.Invoke();
            });
        }

        private void ShowCanvasGroup()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        private void HideCanvasGroup()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void StartShowAnimation()
        {
            BreakAnimation();
            _animationLoop = StartCoroutine(AnimationLoop(0, 1, ShowAnimationTime, null));
        }

        private void StartHideAnimation(Action callback)
        {
            BreakAnimation();
            _animationLoop = StartCoroutine(AnimationLoop(1, 0, HideAnimationTime, callback));
        }

        private void BreakAnimation()
        {
            if (_animationLoop != null)
                StopCoroutine(_animationLoop);
        }

        private IEnumerator AnimationLoop(float from, float to, float time, Action callback)
        {
            float t = 0;
            var scale = _panel.localScale;
            while (t < 1)
            {
                t += Time.deltaTime / time;
                scale.y = Mathf.Lerp(from, to, Ease.EaseIn(t));
                _panel.localScale = scale;
                yield return null;
            }
            callback?.Invoke();
            _animationLoop = null;
        }
    }
}
