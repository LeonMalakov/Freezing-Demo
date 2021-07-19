using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public static class ListExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int index = UnityEngine.Random.Range(0, list.Count);

                T value = list[i];
                list[i] = list[index];
                list[index] = value;
            }

            return list;
        }

        public static IList<T> CopyAndShuffle<T>(this IReadOnlyList<T> list)
        {
            var copied = new List<T>(list.Count);
            for (int i = 0; i < list.Count; i++)
                copied.Add(list[i]);

            return copied.Shuffle();
        }
    }
}
