using System;
using System.Collections;
using UnityEngine;

namespace WGame
{
    public class TreeView : MonoBehaviour
    {
        private const float DyingAnimationDuration = 1.5f;
        private const float SnowAppearingAnimationDuration = 5f;
        private const float RecoverAnimationDuration = 8f;
        [SerializeField] private MeshRenderer _stem;
        [SerializeField] private MeshRenderer _stemSnow;

        private void Awake()
        {
            _stemSnow.enabled = false;
        }

        public void Die(Vector3 fallDirection, Action callback)
        {
            StartCoroutine(DyingAnimation(fallDirection, DyingAnimationDuration,
                callback: () =>
            {
                callback?.Invoke();
                StartCoroutine(SnowAppearingAnimation(SnowAppearingAnimationDuration));
            }));
        }

        public void Recover(Action callback)
        {
            StartCoroutine(RecoverAnimation(RecoverAnimationDuration, callback));
        }

        private IEnumerator DyingAnimation(Vector3 fallDirection, float duration, Action callback)
        {
            Transform stemTransform = _stem.transform;

            Quaternion startRotation = stemTransform.localRotation;
            Quaternion fallOrientation = Quaternion.LookRotation(fallDirection, Vector3.up);
            Quaternion targetRotation = fallOrientation * Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(fallOrientation);
            yield return StartCoroutine(CoroutineAnimation.Rotate(stemTransform, startRotation, targetRotation, Ease.InCubic, duration));

            _stem.enabled = false;
            stemTransform.localRotation = Quaternion.identity;
            callback?.Invoke();
        }

        private IEnumerator SnowAppearingAnimation(float duration)
        {
            _stemSnow.enabled = true;
            yield return StartCoroutine(CoroutineAnimation.Scale(_stemSnow.transform, Vector3.zero, Vector3.one, Ease.OutQuad, duration));
        }

        private IEnumerator RecoverAnimation(float duration, Action callback)
        {
            Transform stemTransform = _stem.transform;
            _stem.enabled = true;

            StartCoroutine(CoroutineAnimation.Scale(_stemSnow.transform, Vector3.one, Vector3.zero, Ease.InSine, duration));
            yield return StartCoroutine(CoroutineAnimation.Scale(stemTransform, Vector3.zero, Vector3.one, Ease.OutSine, duration));

            _stemSnow.enabled = false;

            callback?.Invoke();
        }
    }
}
