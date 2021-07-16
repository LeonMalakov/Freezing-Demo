using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/ItemsFactory")]
    public class ItemsFactory : ScriptableObject
    {
        [SerializeField] private Item _prefab;

        public Item Get()
        {
            var instance = Instantiate(_prefab);
            return instance;
        }
    }
}