using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Foods.Core
{
    public class FoodAnimation : MonoBehaviour
    {
        [SerializeField] private Food _food;
        [SerializeField] private Animator _animator;

        private void OnEnable() => 
            _food.OnCollect += () => 
                _animator.SetTrigger("Collect");
    }
}