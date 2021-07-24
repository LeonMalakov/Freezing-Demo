using UnityEngine;

namespace WGame
{
    public static class Ease
    {
        public static float InSine(float t) => 1 - Mathf.Cos((t * Mathf.PI) / 2);

        public static float OutSine(float t) => Mathf.Sin((t) * Mathf.PI / 2);

        public static float InOutSine(float t) => Mathf.Sin((2 * t - 1) * Mathf.PI / 2) + 0.5f;

        public static float InQuad(float t) => Mathf.Pow(t, 2);

        public static float OutQuad(float t) => 1 - Mathf.Pow(1 - t, 2);

        public static float InCubic(float t) => Mathf.Pow(t, 3);

        public static float OutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);

        public static float InQuart(float t) => Mathf.Pow(t, 4);

        public static float OutQuart(float t) => 1 - Mathf.Pow(1 - t, 4);
    }
}