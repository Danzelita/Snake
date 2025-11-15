using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Scripts
{
    public class SnakeTrail : MonoBehaviour
    {
        [SerializeField] private GameObject _detailPrefab;
        [SerializeField] private float _detailDistance = 1f;
        
        private Transform _head;
        private float _speed;
        private readonly List<Transform> _details = new();
        private readonly List<Vector3> _positionHistory = new();
        private readonly List<Quaternion> _rotationHistory = new();

        public void Init(Transform head, float speed, int detailCount)
        {
            _head = head;
            _speed = speed;
            
            _details.Add(transform);
            _positionHistory.Add(_head.position);
            _rotationHistory.Add(_head.rotation);
            
            _positionHistory.Add(transform.position);
            _rotationHistory.Add(transform.rotation);

            SetDetailCount(detailCount);
        }

        public void Destroy()
        {
            foreach (var detail in _details) 
                Destroy(detail.gameObject);   
        }

        public void SetDetailCount(int detailCount)
        {
            if (detailCount == _details.Count - 1) return;
            
            int diff = _details.Count - 1 - detailCount;
            
            if (diff < 1)
                for (int i = 0; i < -diff; i++)
                    AddDetail();
            else
                for (int i = 0; i < diff; i++)
                    RemoveDetail();
        }

        private void AddDetail()
        {
            Vector3 position = _details[^1].position;
            quaternion rotation = _details[^1].rotation;
            
            GameObject newDetail = Instantiate(_detailPrefab, position, rotation);
            _details.Insert(0, newDetail.transform);
            
            _positionHistory.Add(position);
            _rotationHistory.Add(rotation);
        }

        private void RemoveDetail()
        {
            if (_details.Count <= 1)
            {
                Debug.LogError("Пытаемся удалить деталь, которой нет");
                return;
            }
            
            Transform detail = _details[0];
            _details.Remove(detail);
            Destroy(detail.gameObject);
            _positionHistory.RemoveAt(_positionHistory.Count - 1);
            _rotationHistory.RemoveAt(_rotationHistory.Count - 1);
        }

        private void Update()
        {
            float distance = (_head.position - _positionHistory[0]).magnitude;
            
            while (distance > _detailDistance)
            {
                Vector3 direction = (_head.position - _positionHistory[0]).normalized;
                
                _positionHistory.Insert(0, _positionHistory[0] + direction * _detailDistance);
                _positionHistory.RemoveAt(_positionHistory.Count - 1);
                
                _rotationHistory.Insert(0, _head.rotation);
                _rotationHistory.RemoveAt(_rotationHistory.Count - 1);
                
                distance -= _detailDistance;
            }

            for (int i = 0; i < _details.Count; i++)
            {
                float percent = distance / _detailDistance;
                _details[i].position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], percent);
                _details[i].rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], percent);
            }
        }
    }
}