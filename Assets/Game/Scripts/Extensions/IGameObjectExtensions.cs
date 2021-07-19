using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public static class IGameObjectExtensions
    {
        public static T FindClosest<T>(this IEnumerable<T> targets, Vector3 to) where T : IGameObject
        {
            float minDistance = float.MaxValue;
            T closest = default;
            foreach (var x in targets)
            {
                float dist = Vector3.Distance(x.Transform.position, to);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = x;
                }
            }

            return closest;
        }

        public static IEnumerable<T> Take<T>(this Collider[] hits)
        {
            return hits
                .Select(x => x.GetComponent<T>())
                .Where(x => x != null);
        }
    }
}
