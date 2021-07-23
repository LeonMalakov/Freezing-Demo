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
            var newPlayer = hits.Take<Player>().FirstOrDefault();
            return newPlayer;
        }

        public static IEnumerable<T> GetComponentsViaOverlapSphere<T>(Vector3 position, float radius, LayerMask mask)
        {
            return Physics.OverlapSphere(position, radius, mask).Take<T>();
        }
    }
}
