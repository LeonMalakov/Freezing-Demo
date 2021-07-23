using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        private const float AnimationTime = 0.5f;

        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public async Task Load(params ILoadingOperation[] operations)
        {
            await Show();

            foreach (var operation in operations)
                await operation.Load();

            await Hide();
        }

        private async Task Show()
        {
            var completionSource = new TaskCompletionSource<bool>();
            StartCoroutine(AnimationLoop(0, 1, AnimationTime, completionSource));
            _canvas.enabled = true;
            await completionSource.Task;
        }

        private async Task Hide()
        {
            var completionSource = new TaskCompletionSource<bool>();
            StartCoroutine(AnimationLoop(1, 0, AnimationTime, completionSource));
            await completionSource.Task;
            _canvas.enabled = false;
        }

        private IEnumerator AnimationLoop(float from, float to, float time, TaskCompletionSource<bool> completionSource)
        {
            float t = 0;
            while(t < 1)
            {
                t += Time.deltaTime / time;
                _canvasGroup.alpha = Mathf.Lerp(from, to, Ease.EaseIn(t));
                yield return null;
            }

            completionSource.SetResult(true);
        }
    }
}