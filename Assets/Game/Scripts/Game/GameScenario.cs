using System.Collections.Generic;
using System.Threading.Tasks;
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
        private bool _isUpdating;

        protected Player Player { get; private set; }
        protected Campfire Campfire { get; private set; }
        protected IReadOnlyList<ItemSpawnPoint> ItemsSpawnPoints => _itemsSpawnPoints;
        protected IReadOnlyList<TreeSpawnPoint> TreesSpawnPoints => _treesSpawnPoints;
        protected IReadOnlyList<EnemySpawnPoint> EnemiesSpawnPoints => _enemiesSpawnPoints;
        protected int AveragePlanetRadius => _averagePlanetRadius;
        protected IReadOnlyCollection<Enemy> Enemies => _enemies;

        public void Play()
        {
            OnPlay();
            _isUpdating = true;
        }

        public async Task CleanUpAsync()
        {
            _isUpdating = false;
            _enemies.Clear();
            await Game.CleanUpAsync();
        }

        public void EnemyRemoved(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        private void FixedUpdate()
        {
            if (_isUpdating)
                OnFixedUpdate();
        }

        protected void ExitToMainMenu()
        {
            Debug.Log("exit");
        }

        protected void RestartGame()
        {
            Loading.RestartGame(this);
        }

        protected void CreateEnemy(EnemySpawnPoint spawnPoint)
        {
            var enemy = Game.CreateEnemy(spawnPoint.transform.position, spawnPoint.transform.rotation);
            _enemies.Add(enemy);
        }

        protected void CreateMain()
        {
            CreatePlayer();
            CreateCampfire();
        }

        private void CreatePlayer()
        {
            Player = Game.CreatePlayer(_playerSpawnPoint.transform.position, _playerSpawnPoint.transform.rotation);
            _dependencies.Set(Player);
            Player.Died += OnPlayerDied;
        }

        private void CreateCampfire()
        {
            Campfire = Game.CreateCampfire(_campfireSpawnPoint.transform.position, _campfireSpawnPoint.transform.rotation);
            _dependencies.Set(Campfire);
        }

        private void OnDestroy()
        {
            if (Player != null)
                Player.Died -= OnPlayerDied;
        }

        protected abstract void OnPlay();
        protected abstract void OnFixedUpdate();
        protected abstract void OnPlayerDied();
    }
}