using System;
using UnityEngine;

namespace WGame
{
    public class Dependencies : MonoBehaviour
    {
        [SerializeField] private Component[] _playerTargetings;
        [SerializeField] private Component[] _campfireTargetings;

        public void Set(Player player)
        {
            foreach (var x in _playerTargetings)
                (x as IPlayerTargeting).SetTarget(player);
        }

        public void Set(Campfire campfire)
        {
            foreach (var x in _campfireTargetings)
                (x as ICampfireTargeting).SetTarget(campfire);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            GetInterfaceComponents(_playerTargetings, typeof(IPlayerTargeting));
            GetInterfaceComponents(_campfireTargetings, typeof(ICampfireTargeting));
        }

        private void GetInterfaceComponents(Component[] array, Type type)
        {
            if (array == null) return;

            for (int i = 0; i < array.Length; i++)
                if (array[i] != null)
                {
                    var value = array[i].GetComponent(type);
                    array[i] = value;
                }
        }
#endif
    }
}