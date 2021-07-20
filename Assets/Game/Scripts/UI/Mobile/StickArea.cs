using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

namespace WGame.Mobile
{
    public class StickArea : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private CanvasGroup _stickAnchor;
        [SerializeField] private OnScreenStick _stick;

        private bool _isStickEnabled;

        private void Awake()
        {
            DisableStick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isStickEnabled == false)
            {
                EnableStick(eventData);
            }

            _stick.OnPointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isStickEnabled == false)
            {
                EnableStick(eventData);
            }

            _stick.OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _stick.OnPointerUp(eventData);

            if (_isStickEnabled)
            {
                DisableStick();
            }
        }

        private void EnableStick(PointerEventData eventData)
        {
            _isStickEnabled = true;
            _stickAnchor.GetComponent<RectTransform>().position = eventData.position;
            _stickAnchor.alpha = 1;
            _stickAnchor.blocksRaycasts = true;
        }

        private void DisableStick()
        {
            _isStickEnabled = false;
            _stickAnchor.alpha = 0;
            _stickAnchor.blocksRaycasts = false;
        }
    }
}