using UnityEngine;

namespace WGame
{
    public static class Ease
    {
        public static float EaseIn(float t) => Mathf.Sin((t - 1) * Mathf.PI / 2) + 1;

        public static float EaseOut(float t) => Mathf.Sin((t) * Mathf.PI / 2);

        public static float EaseInOut(float t) => Mathf.Sin((2 * t - 1) * Mathf.PI / 2) + 0.5f;
    }
}