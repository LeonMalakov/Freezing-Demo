using UnityEngine;

namespace WGame
{
    public class SurvivalScenario : GameScenario
    {
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        [SerializeField] private CampfireSpawnPoint _campfireSpawnPoint;
        [SerializeField] private ItemSpawnPoint[] _itemsSpawnPoints;
        [SerializeField] private TreeSpawnPoint[] _treesSpawnPoints;
        [SerializeField] private EnemySpawnPoint[] _enemiesSpawnPoints;
        [SerializeField] private PlayerFollower _camera;

        public override void Play()
        {
            CreateLevel();
        }

        private void CreateLevel()
        {
            var player = Game.CreatePlayer(_playerSpawnPoint.transform.position, _playerSpawnPoint.transform.rotation);
            Game.CreateCampfire(_campfireSpawnPoint.transform.position, _campfireSpawnPoint.transform.rotation);
            CreateItems();
            CreateTrees();
            CreateEnemies();

            _camera.Init(player);
        }

        private void CreateTrees()
        {
            foreach (var tree in _treesSpawnPoints)
                Game.CreateTree(tree.transform.position, tree.transform.rotation);
        }

        private void CreateItems()
        {
            foreach (var item in _itemsSpawnPoints)
                Game.CreateItem(item.transform.position, item.transform.rotation);
        }

        private void CreateEnemies()
        {
            foreach (var item in _enemiesSpawnPoints)
                Game.CreateEnemy(item.transform.position, item.transform.rotation);
        }
    }
}
