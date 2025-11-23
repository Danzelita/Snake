using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.Snakes.Core;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Animation
{
    public class SnakeTrailAnimation : MonoBehaviour
    {
        [SerializeField] private float _waitHeadTime;
        [SerializeField] private SnakeTrail _snakeTrail;
        [SerializeField] private AnimationCurve _animatonDurationByDetailsCount;

        private void OnEnable() => 
            _snakeTrail.DetailsChanged += OnDetailsChanged;

        private void OnDisable() => 
            _snakeTrail.DetailsChanged -= OnDetailsChanged;

        private void OnDetailsChanged(List<Transform> details)
        {
            float animationDuration = _animatonDurationByDetailsCount.Evaluate(details.Count);
            
            for (int i = 0; i < details.Count; i++)
            {
                float dutation = animationDuration / details.Count;
                float delay = dutation * i;
                SnakeDetailAnimation snakeDetailAnimation = details[i].GetComponent<SnakeDetailAnimation>();
                snakeDetailAnimation.PlayAnimation(delay + _waitHeadTime, dutation);
            }
            float trailDelay = animationDuration / details.Count * (details.Count - 1);
            GetComponent<SnakeDetailAnimation>().DisableModel(delay: trailDelay + _waitHeadTime);
        }
    }
}