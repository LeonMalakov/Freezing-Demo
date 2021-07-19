using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName = "Factories/PlayerFactory")]
    public class PlayerFactory : GameObjectFactory
    {
        [SerializeField] private Player _prefab;

        public Player Get()
        {
            var instance = CreateGameObjectInstance(_prefab);
            return instance;
        }

        public void Reclaim(Player player)
        {
            Destroy(player.gameObject);
        }
    }
}
