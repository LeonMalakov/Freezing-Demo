using System.Threading.Tasks;
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

        private static Game instance;

        public void Init()
        {
            instance = this;
        }

        public static async Task Play()
        {
            instance._scenario.Play();
            await Task.Delay(1);
        }

        public static async Task CleanUpAsync()
        {
            await Task.WhenAll(
                CleanUpPlayerAsync(),
                CleanUpCampfireAsync(),
                CleanUpEnemiesAsync(),
                CleanUpTreesAsync(),
                CleanUpItemsAsync());
        }

        public static Player CreatePlayer(Vector3 position, Quaternion rotation)
        {
            var instance = Game.instance._playerFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static void RemovePlayer(Player toRemove)
        {
            instance._playerFactory.Reclaim(toRemove);
        }

        public static Campfire CreateCampfire(Vector3 position, Quaternion rotation)
        {
            var instance = Game.instance._campfireFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static void RemoveCampfire(Campfire toRemove)
        {
            instance._campfireFactory.Reclaim(toRemove);
        }

        public static Item CreateItem(Vector3 position, Quaternion rotation)
        {
            var instance = Game.instance._itemsFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static void RemoveItem(Item toRemove)
        {
            instance._itemsFactory.Reclaim(toRemove);
        }

        public static Tree CreateTree(Vector3 position, Quaternion rotation)
        {
            var instance = Game.instance._treesFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static void RemoveTree(Tree toRemove)
        {
            instance._treesFactory.Reclaim(toRemove);
        }

        public static Enemy CreateEnemy(Vector3 position, Quaternion rotation)
        {
            var instance = Game.instance._enemiesFactory.Get();
            PlaceInstance(position, rotation, instance);
            return instance;
        }

        public static void RemoveEnemy(Enemy toRemove)
        {
            instance._scenario.EnemyRemoved(toRemove);
            instance._enemiesFactory.Reclaim(toRemove);
        }

        public static async Task CleanUpPlayerAsync() => await instance._playerFactory.CleanUpAsync();

        public static async Task CleanUpCampfireAsync() => await instance._campfireFactory.CleanUpAsync();

        public static async Task CleanUpEnemiesAsync() => await instance._enemiesFactory.CleanUpAsync();

        public static async Task CleanUpItemsAsync() => await instance._itemsFactory.CleanUpAsync();

        public static async Task CleanUpTreesAsync() => await instance._treesFactory.CleanUpAsync();

        private static void PlaceInstance(Vector3 position, Quaternion rotation, GameBehaviour instance)
        {
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.EarthPlacer.Place();
        }
    }
}