using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/ItemsFactory")]
    public class ItemsFactory : GameObjectFactory
    {
        [SerializeField] private Item _prefab;

        public Item Get()
        {
            var instance = CreateGameObjectInstance(_prefab);
            return instance;
        }

        public void Reclaim(Item item)
        {
            Destroy(item.gameObject);
        }
    }
}