using System;
using Project.Scripts.Gameplay.Controller;
using Project.Scripts.Gameplay.Snakes.Core;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Animation
{
    public class SnakeHeadAnimation : MonoBehaviour
    {
        [SerializeField] private Snake _snake;
        [SerializeField] private Animator _animator;
        
        private const string Collect = "Collect";

        private void OnEnable() =>
            _snake.OnCollect += () => 
                _animator.SetTrigger(Collect);
    }
}