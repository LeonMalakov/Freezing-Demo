using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WGame
{
    public abstract class TargetPointerView : MonoBehaviour, IPlayerTargeting
    {
        [SerializeField] private Image _image;

        private Coroutine _updateTargetLoop;

        protected Player Player { get; private set; }

        public void SetTarget(Player player)
        {
            TryUnsubscribe();
            Player = player;

            if (isActiveAndEnabled)
                TrySubscribe();
        }

        private void OnEnable()
        {
            _image.enabled = false;
            TrySubscribe();
        }

        private void OnDisable()
        {
            TryUnsubscribe();
        }

        private void TrySubscribe()
        {
            if (Player != null)
                Subscribe();
        }

        private void TryUnsubscribe()
        {
            if (Player != null)
                Unsubscribe();
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        protected void OnActiveChanged(IGameObject target)
        {
            if (_updateTargetLoop != null)
                StopCoroutine(_updateTargetLoop);

            if (target != null)
                _updateTargetLoop = StartCoroutine(UpdateTargetLoop(target));
            else
                _image.enabled = false;           
        }

        private IEnumerator UpdateTargetLoop(IGameObject target)
        {
            _image.enabled = true;

            float t = 0;
            while (UpdatePointerWhile(t))
            {
                t += Time.deltaTime;
                _image.rectTransform.position = Camera.main.WorldToScreenPoint(target.Transform.position);
                yield return null;
            }

            _image.enabled = false;
            _updateTargetLoop = null;
        }

        protected abstract bool UpdatePointerWhile(float t);
    }
}
