using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName ="Factories/TreesFactory")]
    public class TreesFactory : GameObjectFactory
    {
        [SerializeField] private Tree[] _prefabs;

        public Tree Get()
        {
            var instance = CreateGameObjectInstance(RandomPrefab());
            instance.Init();
            return instance;
        }

        public void Reclaim(Tree tree)
        {
            Destroy(tree.gameObject);
        }

        private Tree RandomPrefab() => _prefabs[Random.Range(0, _prefabs.Length)];
    }
}