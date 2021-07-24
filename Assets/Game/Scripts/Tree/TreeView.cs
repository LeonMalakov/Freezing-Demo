using System;
using System.Collections;
using UnityEngine;

namespace WGame
{
    public class TreeView : MonoBehaviour
    {
        [SerializeField] private GameObject _stem;

        public void Die(Action callback)
        {
            StartCoroutine(DyingAnimation(1.5f, callback));
        }

        public void Recover(Action callback)
        {
            StartCoroutine(RecoverAnimation(3f, callback));
        }

        private IEnumerator DyingAnimation(float duration, Action callback)
        {
            Transform stem = _stem.transform;

            float t = 0;
            Quaternion startRotation = stem.localRotation;
            Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
            while(t < 1)
            {
                t += Time.deltaTime / duration;
                stem.localRotation = Quaternion.Lerp(startRotation, targetRotation, Ease.InCubic(t));
                yield return null;
            }

            _stem.SetActive(false);
            callback?.Invoke();
        }

        private IEnumerator RecoverAnimation(float duration, Action callback)
        {
            Transform stem = _stem.transform;
            _stem.SetActive(true);

            float t = 0;
            stem.localRotation = Quaternion.identity;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                stem.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Ease.OutQuad(t));
                yield return null;
            }

            callback?.Invoke();
        }
    }
}
