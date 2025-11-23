using System.Collections;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Animation
{
    public class SnakeDetailAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _yScaleCurve;
        [SerializeField] private Transform _model;
        [SerializeField] private float _overDuration = 0.1f;

        public void PlayAnimation(float delay, float duration) =>
            StartCoroutine(AnimationProcesss(delay, duration));

        public void DisableModel(float delay) =>
            StartCoroutine(DisableModelProcess(delay));

        private IEnumerator DisableModelProcess(float delay)
        {
            _model.gameObject.SetActive(false);
            yield return new WaitForSeconds(delay);
            _model.gameObject.SetActive(true);
        }

        private IEnumerator AnimationProcesss(float delay, float duration)
        {
            yield return new WaitForSeconds(delay);

            for (float t = 0; t < 1f; t += Time.deltaTime / (duration + _overDuration))
            {
                float y = _yScaleCurve.Evaluate(t);
                _model.localScale = new Vector3(1f, y, 1f);
                yield return null;
            }

            _model.localScale = Vector3.one;
        }
    }
}