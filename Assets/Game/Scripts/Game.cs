using UnityEngine;

namespace WGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private ItemsFactory _itemsFactory;
        [SerializeField] private TreesFactory _treesFactory;
        [SerializeField] private EnemiesFactory _enemiesFactory;
        [SerializeField] private ItemSpawnPoint[] _itemsSpawnPoints;
        [SerializeField] private TreeSpawnPoint[] _treesSpawnPoints;
        [SerializeField] private EnemySpawnPoint[] _enemiesSpawnPoints;

        private static Game _instance;

        private void Start()
        {
            CreateLevel();
        }

        private void OnEnable()
        {
            _instance = this;
        }

        public void CreateLevel()
        {
            CreateItems();
            CreateTrees();
            CreateEnemies();
        }

        private void CreateTrees()
        {
            foreach (var tree in _treesSpawnPoints)
                CreateTree(tree.transform.position, tree.transform.rotation);
        }

        private void CreateItems()
        {
            foreach (var item in _itemsSpawnPoints)
                CreateItem(item.transform.position, item.transform.rotation);
        }

        private void CreateEnemies()
        {
            foreach (var item in _enemiesSpawnPoints)
                CreateEnemy(item.transform.position, item.transform.rotation);
        }

        public static Item CreateItem(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._itemsFactory.Get();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.EarthPlacer.Place();
            return instance;
        }

        public static Tree CreateTree(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._treesFactory.Get();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.EarthPlacer.Place();
            return instance;
        }

        public static Enemy CreateEnemy(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._enemiesFactory.Get();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.EarthPlacer.Place();
            return instance;
        }
    }
}