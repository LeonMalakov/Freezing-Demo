using UnityEngine;

namespace WGame
{
    public class Dependencies : MonoBehaviour
    {
        [SerializeField] private Component[] _playerTargetings;

#if UNITY_EDITOR
        private void OnValidate()
        {
            GetPlayerTargetings();
        }

        private void GetPlayerTargetings()
        {
            for (int i = 0; i < _playerTargetings.Length; i++)
                if (_playerTargetings[i] != null)
                {
                    var value = _playerTargetings[i].GetComponent(typeof(IPlayerTargeting));
                    _playerTargetings[i] = value;
                }
        }
#endif

        public void Set(Player player)
        {
            foreach (var x in _playerTargetings)
                (x as IPlayerTargeting).SetTarget(player);
        }
    }
}