using UnityEngine;

namespace WGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameScenario _scenario;
        [SerializeField] private PlayerFactory _playerFactory;
        [SerializeField] private CampfireFactory _campfireFactory;
        [SerializeField] private ItemsFactory _itemsFactory;
        [SerializeField] private TreesFactory _treesFactory;
        [SerializeField] private EnemiesFactory _enemiesFactory;

        private static Game _instance;

        private void Start()
        {
            _scenario.Play();
        }

        private void OnEnable()
        {
            _instance = this;
        }

        public static Player CreatePlayer(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._playerFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static Campfire CreateCampfire(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._campfireFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static Item CreateItem(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._itemsFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static Tree CreateTree(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._treesFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static Enemy CreateEnemy(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._enemiesFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static void RemoveEnemy(Enemy enemy)
        {
            _instance._scenario.EnemyRemoved(enemy);
            _instance._enemiesFactory.Reclaim(enemy);
        }

        private static void PlaceInstance(Vector3 position, Quaternion rotation, GameBehaviour instance)
        {
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.EarthPlacer.Place();
        }
    }
}