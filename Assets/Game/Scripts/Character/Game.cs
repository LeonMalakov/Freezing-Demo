using UnityEngine;

namespace WGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private ItemsFactory _itemsFactory;

        private static Game _instance;

        private void OnEnable()
        {
            _instance = this;
        }

        public static Item CreateItem(Vector3 position, Quaternion rotation)
        {
            var instance = _instance._itemsFactory.Get();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.EarthPlacer.Place();
            return instance;
        }
    }
}