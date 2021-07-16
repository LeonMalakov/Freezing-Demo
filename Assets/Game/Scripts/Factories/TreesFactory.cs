using UnityEngine;

namespace WGame
{
    [CreateAssetMenu(menuName ="Factories/TreesFactory")]
    public class TreesFactory : GameObjectFactory, ITreesOwner
    {
        [SerializeField] private Tree _tree;

        public Tree Get()
        {
            var instance = CreateGameObjectInstance(_tree);
            instance.OriginFactory = this;
            return instance;
        }

        public void Reclaim(Tree tree)
        {
            Destroy(tree.gameObject);
        }
    }
}