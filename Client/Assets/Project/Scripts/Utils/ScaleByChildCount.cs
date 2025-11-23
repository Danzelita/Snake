using System;
using UnityEngine;

namespace Project.Scripts.Utils
{
    public class ScaleByChildCount : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private float _add;
        [SerializeField] private float _multiplayer;
        private RectTransform _rectTranfrom;

        private void Awake()
        {
            _rectTranfrom = GetComponent<RectTransform>();
        }

        private void Update()
        {
            int count = 0;

            for (int i = 0; i < _parent.childCount; i++)
                if (_parent.GetChild(i).gameObject.activeSelf)
                {
                    count++;
                }
            
            float ySize = count * _multiplayer + _add;
            _rectTranfrom.sizeDelta = new Vector2(_rectTranfrom.sizeDelta.x, ySize);
        }
    }
}