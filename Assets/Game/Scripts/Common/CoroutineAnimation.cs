using System;
using System.Collections;
using UnityEngine;

namespace WGame
{
    public static class CoroutineAnimation
    {
        public static IEnumerator Scale(Transform target, Vector3 from, Vector3 to, Func<float, float> ease, float duration)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                target.localScale = Vector3.Lerp(from, to, ease(t));
                yield return null;
            }
        }

        public static IEnumerator Rotate(Transform target, Quaternion from, Quaternion to, Func<float, float> ease, float duration)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                target.localRotation = Quaternion.Lerp(from, to, ease(t));
                yield return null;
            }
        }
    }
}
