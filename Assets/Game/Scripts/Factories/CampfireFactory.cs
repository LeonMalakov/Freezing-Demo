using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/CampfireFactory")]
    public class CampfireFactory : GameObjectFactory
    {
        [SerializeField] private Campfire _prefab;

        public Campfire Get()
        {
            var instance = CreateGameObjectInstance(_prefab);
            instance.Init();
            return instance;
        }

        public void Reclaim(Campfire campfire)
        {
            Destroy(campfire.gameObject);
        }
    }
}
