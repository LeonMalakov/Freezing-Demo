using UnityEngine;
using UnityEngine.UI;

namespace WGame.UI
{
    public class Checkbox : MonoBehaviour
    {
        [SerializeField] private Image _onSign;
        [SerializeField] private bool _isOn;

        public bool IsOn => _isOn;

        public void SetValue(bool isOn)
        {
            _isOn = isOn;
            UpdateSign();
        }

        private void UpdateSign()
        {
            if (_onSign != null)
                _onSign.enabled = _isOn;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateSign();
        }
#endif
    }
}