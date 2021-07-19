using System;
using UnityEngine;

namespace WGame.UI
{
    [ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _fill;
        [SerializeField] [Range(0, 1)] private float _progress;

        public float Progress => _progress;

        public void SetProgress(float vaule)
        {
            if (vaule < 0 || vaule > 1)
                throw new ArgumentOutOfRangeException();

            _progress = vaule;
            UpdateFill();
        }

        private void UpdateFill()
        {
            if (_fill != null)
                _fill.sizeDelta = new Vector2(_progress * GetComponent<RectTransform>().sizeDelta.x, _fill.sizeDelta.y);
        }

#if UNITY_EDITOR
        private bool _update;

        private void OnValidate()
        {
            _update = true;
        }

        private void LateUpdate()
        {
            if (_update)
            {
                UpdateFill();
                _update = false;
            }
        }
#endif
    }
}