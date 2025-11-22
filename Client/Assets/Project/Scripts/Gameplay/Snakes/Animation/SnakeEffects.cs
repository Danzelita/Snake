using System;
using Project.Scripts.Settings.Skins;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Animation
{
    public class SnakeEffects : MonoBehaviour
    {
        [SerializeField] private SnakeDeathParticle _snakeDeathParticle;
        private SkinSettings _skinSettings;

        public void Init(SkinSettings skinSettings)
        {
            _skinSettings = skinSettings;
        }

        private void OnDestroy()
        {
            var snakeDeathParticle = Instantiate(_snakeDeathParticle, transform.position, Quaternion.identity);
            snakeDeathParticle.Init(_skinSettings.Material);
        }
    }
}