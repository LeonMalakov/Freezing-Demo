using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/ItemsFactory")]
    public class ItemsFactory : GameObjectFactory, IItemsOwner
    {
        [SerializeField] private Item _prefab;

        public Item Get()
        {
            var instance = CreateGameObjectInstance(_prefab);
            instance.OriginFactory = this;
            return instance;
        }

        public void Reclaim(Item item)
        {
            Destroy(item.gameObject);
        }
    }
}