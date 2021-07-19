using UnityEngine;

namespace WGame
{
    public class TreeView : MonoBehaviour
    {
        [SerializeField] private GameObject _stem;

        public void Die()
        {
            _stem.SetActive(false);
        }

        public void Recover()
        {
            _stem.SetActive(true);
        }
    }
}
