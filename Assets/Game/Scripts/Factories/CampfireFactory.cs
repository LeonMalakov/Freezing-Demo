using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/CampfireFactory")]
    public class CampfireFactory : GameObjectFactory, ICampfireOwner
    {
        [SerializeField] private Campfire _prefab;

        public Campfire Get()
        {
            var instance = CreateGameObjectInstance(_prefab);
            instance.OriginFactory = this;
            return instance;
        }

        public void Reclaim(Campfire campfire)
        {
            Destroy(campfire.gameObject);
        }
    }
}
