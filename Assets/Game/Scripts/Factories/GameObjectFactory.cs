using System.Threading.Tasks;
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

        public async Task CleanUpAsync()
        {
            if (_container != null)
            {
                Destroy(_container.gameObject);

                while (_container != null)
                    await Task.Delay(1);

                Debug.Log($"{name} cleaned up");
            }
        }
    }
}
