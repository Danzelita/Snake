using Project.Scripts.Gameplay.Controller;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Animation
{
    public class SnakeHeadAnimation : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private Animator _animator;
        
        private const string Collect = "Collect";
    }
}