using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/EnemiesFactory")]
    public class EnemiesFactory : GameObjectFactory
    {
        [SerializeField] private Enemy _prefab;

        public Enemy Get()
        {
            var instance = CreateGameObjectInstance(_prefab);
            instance.Init();
            return instance;
        }

        public void Reclaim(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}