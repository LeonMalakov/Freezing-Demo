using System.Linq;
using UnityEngine;

namespace WGame
{
    public class SurvivalScenario : GameScenario
    {
        [SerializeField] [Range(0, 100)] private int _targetEnemyCount = 6;

        public override void Play()
        {
            CreateLevel();
        }

        private void CreateLevel()
        {
            CreateMain();
            CreateItems();
            CreateTrees();
            CreateEnemies();
        }

        private void FixedUpdate()
        {
            if (Enemies.Count < _targetEnemyCount)
            {
                TrySpawnEnemyAtRandomPoint();
            }
        }

        private bool TrySpawnEnemyAtRandomPoint()
        {
            var spawnPoint = RandomEnemySpawnPointAtPlanetRadiusDistance();
            if (spawnPoint != null)
            {
                SpawnEnemy(spawnPoint);
                return true;
            }

            return false;
        }

        private void CreateTrees()
        {
            foreach (var tree in TreesSpawnPoints)
                Game.CreateTree(tree.transform.position, tree.transform.rotation);
        }

        private void CreateItems()
        {
            foreach (var item in ItemsSpawnPoints)
                Game.CreateItem(item.transform.position, item.transform.rotation);
        }

        private void CreateEnemies()
        {
            var spawnPoints = EnemiesSpawnPoints.CopyAndShuffle();
            int counter = 0;

            while (Enemies.Count < _targetEnemyCount)
            {
                SpawnEnemy(spawnPoints[counter++]);

                if (counter >= spawnPoints.Count)
                    counter = 0;
            }
        }

        private EnemySpawnPoint RandomEnemySpawnPointAtPlanetRadiusDistance()
        {
            var sp = EnemiesSpawnPoints
                .Where(x => Vector3.Distance(x.transform.position, Player.transform.position) > AveragePlanetRadius);

            return sp.ElementAt(Random.Range(0, sp.Count()));
        }
    }
}
