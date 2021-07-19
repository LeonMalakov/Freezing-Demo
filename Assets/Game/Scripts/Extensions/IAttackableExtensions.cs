using System.Collections.Generic;
using System.Linq;

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
    }
}
