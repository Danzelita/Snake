using System;
using System.Collections;
using Colyseus.Schema;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.Logic
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _offsetYByDetailsCount;
        [SerializeField] private float _lerp;
        [SerializeField] private float _flyYSpeed;

        private Transform _target;
        private Transform _camera;
        private float _offsetY;

        public void SetTarget(Transform target, Player player)
        {
            _offsetY = _offsetYByDetailsCount.Evaluate(player.details);
            player.OnChange += changes =>
            {
                foreach (DataChange change in changes)
                    if (change.Field == nameof(player.details))
                        _offsetY = _offsetYByDetailsCount.Evaluate((byte)change.Value);
            };

            _camera = Camera.main.transform;
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target != null)
            {
                Vector3 targetPosition = _target.position + -_camera.forward * _offsetY;
                _camera.position = Vector3.Lerp(_camera.position, targetPosition, _lerp * Time.deltaTime);
            }
            else if (_camera != null)
            {
                _camera.position += Vector3.up * _flyYSpeed * Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            Camera camera = Camera.main;

            if (camera == null) return;

            camera.transform.parent = null;
        }
    }
}