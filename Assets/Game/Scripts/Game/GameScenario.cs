using System.Collections.Generic;
using UnityEngine;

namespace WGame
{
    public abstract class GameScenario : MonoBehaviour
    {
        [SerializeField] private Dependencies _dependencies;
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        [SerializeField] private CampfireSpawnPoint _campfireSpawnPoint;
        [SerializeField] private ItemSpawnPoint[] _itemsSpawnPoints;
        [SerializeField] private TreeSpawnPoint[] _treesSpawnPoints;
        [SerializeField] private EnemySpawnPoint[] _enemiesSpawnPoints;
        [SerializeField] private int _averagePlanetRadius = 25;

        private HashSet<Enemy> _enemies = new HashSet<Enemy>();

        protected Player Player { get; private set; }
        protected Campfire Campfire { get; private set; }
        protected IReadOnlyList<ItemSpawnPoint> ItemsSpawnPoints => _itemsSpawnPoints;
        protected IReadOnlyList<TreeSpawnPoint> TreesSpawnPoints => _treesSpawnPoints;
        protected IReadOnlyList<EnemySpawnPoint> EnemiesSpawnPoints => _enemiesSpawnPoints;
        protected int AveragePlanetRadius => _averagePlanetRadius;
        protected IReadOnlyCollection<Enemy> Enemies => _enemies;

        public abstract void Play();

        public void EnemyRemoved(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        protected void SpawnEnemy(EnemySpawnPoint spawnPoint)
        {
            var enemy = Game.CreateEnemy(spawnPoint.transform.position, spawnPoint.transform.rotation);
            _enemies.Add(enemy);
        }

        protected void CreateMain()
        {
            Player = Game.CreatePlayer(_playerSpawnPoint.transform.position, _playerSpawnPoint.transform.rotation);
            Campfire = Game.CreateCampfire(_campfireSpawnPoint.transform.position, _campfireSpawnPoint.transform.rotation);

            _dependencies.Set(Player);
            _dependencies.Set(Campfire);
        }
    }
}