using Project.Scripts.Gameplay.Snakes.Skins;
using Project.Scripts.Logic;
using Project.Scripts.Settings;
using Project.Scripts.Settings.Skins;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Core
{
    public class Snake : MonoBehaviour
    {
        public float Speed => _speed;
        
        [SerializeField] private SnakeTrail _trailPrefab;
        [SerializeField] private Transform _head;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _rotateSpeed = 90f;
        private SnakeTrail _trail;

        public void Init(SkinSettings skinSettings, int detailCount)
        {
            _trail = Instantiate(_trailPrefab, _head.position, Quaternion.identity);
            _trail.Init(_head, skinSettings, detailCount);
            
            GetComponent<SkinDisplay>().SetSkin(skinSettings);
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
            Quaternion.LookRotation(toPoint, Vector3.up);
            
            _head.rotation = Quaternion.LookRotation(toPoint, Vector3.up);
        }
        
        private void Move() => 
            transform.position += _head.forward * Time.deltaTime * _speed;
    }
}