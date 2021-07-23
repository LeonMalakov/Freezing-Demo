using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public static class CollisionUtilities
    {
        public static Player GetPlayerViaOverlapSphere(Vector3 position, float radius)
        {
            var hits = Physics.OverlapSphere(position, radius, Constants.Layers.Character);
            var newPlayer = hits.Select(x => x.GetComponent<Player>()).FirstOrDefault(x => x != null);
            return newPlayer;
        }

        public static IEnumerable<T> GetComponentsViaOverlapSphere<T>(Vector3 position, float radius, LayerMask mask) where T : MonoBehaviour
        {
            return Physics.OverlapSphere(position, radius, mask)
                .Select(x => x.GetComponent<T>())
                .Where(x => x != null);
        }
    }
}
