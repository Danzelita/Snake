using UnityEngine;

namespace Project.Scripts
{
    public class Snake : MonoBehaviour
    {
        public float Speed => _speed;
        
        [SerializeField] private SnakeTrail _trailPrefab;
        [SerializeField] private Transform _head;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _rotateSpeed = 90f;
        private Quaternion _targetRotation;
        private SnakeTrail _trail;

        public void Init(int detailCount)
        {
            _trail = Instantiate(_trailPrefab, _head.position, Quaternion.identity);
            _trail.Init(_head, _speed, detailCount);
        }
        
        public void SetDetailCount(byte detailCount) =>
            _trail.SetDetailCount(detailCount);
        
        private void Update() => 
            Move();

        public void Destroy()
        {
            Destroy(gameObject);
            _trail.Destroy();
        }

        public void SetRotation(Vector3 point)
        {
            Vector3 toPoint = point - _head.position;
            _targetRotation = Quaternion.LookRotation(toPoint, Vector3.up);
            
            _head.rotation = Quaternion.LookRotation(toPoint, Vector3.up);
        }
        
        private void Move() => 
            transform.position += _head.forward * Time.deltaTime * _speed;
    }
}