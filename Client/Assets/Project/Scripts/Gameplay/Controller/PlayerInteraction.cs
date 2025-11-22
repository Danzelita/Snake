using System;
using Project.Scripts.Gameplay.Foods.Core;
using Project.Scripts.Gameplay.Snakes.Core;
using Project.Scripts.Gameplay.Snakes.Services;
using UnityEngine;

namespace Project.Scripts.Gameplay.Controller
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionLayer;
        [SerializeField] private float _overlapRadius = 0.5f;
        private Collider[] _colliders = new Collider[5];
        private Transform _snakeHead;
        private SnakeService _snakeService;

        public void Init(Transform snakeHead, SnakeService snakeService)
        {
            _snakeHead = snakeHead;
            _snakeService = snakeService;
        }

        private void FixedUpdate()
        {
            CheckCollision();
            CheckExit();
        }

        private void CheckExit()
        {
            if (Mathf.Abs(_snakeHead.position.x) > 128f || Mathf.Abs(_snakeHead.position.z) > 128f) 
                GameOver();
        }

        private void CheckCollision()
        {
            int count = Physics.OverlapSphereNonAlloc(_snakeHead.position, _overlapRadius, _colliders, _collisionLayer);

            for (int i = 0; i < count; i++)
            {
                if (_colliders[i].TryGetComponent(out Food food))
                    food.Collect();
                else
                {
                    if (_colliders[i].GetComponentInParent<Snake>())
                    {
                        Transform enemy = _colliders[i].transform;
                        float playerAngle = Vector3.Angle(enemy.position - _snakeHead.position, _snakeHead.forward);
                        float enemyAngle = Vector3.Angle(_snakeHead.position - enemy.position, enemy.forward);
                        if (playerAngle < enemyAngle + 5f) 
                            GameOver();
                    }
                    else
                        GameOver();
                }
            }
        }

        private void GameOver()
        {
            _snakeService.DestroyPlayer();
        }
    }
}