using UnityEngine;

namespace WGame
{
    public class GameObjectFactory : ScriptableObject
    {
        private Transform _container;

        protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
        {
            if (_container == null)
            {
                _container = new GameObject(name).transform;
            }

            T instance = Instantiate(prefab);
            instance.transform.parent = _container;
            return instance;
        }
    }
}
