using UnityEngine;

namespace Project.Scripts.Gameplay.Controller
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 90f;
        
        private Vector3 _targetDirection;
        private float _speed;

        public void Init(float speed) => 
            _speed = speed;

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Rotate()
        {
            if (_targetDirection == Vector3.zero)
                return;
            
            Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private void Move() => 
            transform.position += transform.forward * _speed * Time.deltaTime;

        public void SetTargetDirection(Vector3 point) => 
            _targetDirection = point - transform.position;

        public void GetMoveInfo(out Vector3 position) => 
            position = transform.position;

        public void Destroy() => 
            Destroy(gameObject);
    }
}