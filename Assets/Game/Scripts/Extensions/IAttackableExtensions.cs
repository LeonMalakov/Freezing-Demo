using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public static class IAttackableExtensions
    {
        public static IEnumerable<IAttackable> TakeAlive(this IEnumerable<IAttackable> targets)
        {
            return targets.Where(x => x.IsAlive);
        }

        public static IEnumerable<IAttackable> TakePrioritiesIfExist(this IEnumerable<IAttackable> targets)
        {
            var priorities = targets.Where(x => x.IsPriority);
            if (priorities.Count() > 0)
                return priorities;
            else
                return targets;
        }

        public static IEnumerable<IAttackable> TakeAtSector(this IEnumerable<IAttackable> targets, Vector3 position, Vector3 direction, float angle)
        {
            return targets
                .Where(x => Vector3.Angle(x.Transform.position - position, direction) < angle * 0.5f);
        }
    }
}
