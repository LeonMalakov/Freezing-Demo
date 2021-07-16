using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/EnemiesFactory")]
    public class EnemiesFactory : GameObjectFactory, IEnemiesOwner
    {
        [SerializeField] private Enemy _prefab;

        public Enemy Get()
        {
            var instance = CreateGameObjectInstance(_prefab);
            instance.OriginFactory = this;
            return instance;
        }

        public void Reclaim(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}