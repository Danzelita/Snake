using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screens.Gameplay.Minimap
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private Image _coloredImage;
        [SerializeField] private Image _coloredLight;

        private RectTransform _rectTransform;

        public void Init(Color color)
        {
            _coloredImage.color = color;
            _coloredLight.color = color;
        }

        public void SetPosition(Vector2 markerPosition)
        {
            if (_rectTransform != null) 
                _rectTransform.anchoredPosition = markerPosition;
        }

        private void Awake() =>
            _rectTransform = GetComponent<RectTransform>();
    }
}