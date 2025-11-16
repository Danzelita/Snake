using UnityEngine;

namespace Project.Scripts.Logic
{
    public class CameraManager : MonoBehaviour
    {

        private Transform _target;
        private Transform _cameraTransform;
        private float _offsetY;

        public void SetTarget(Transform target, float offsetY)
        {
            _offsetY = offsetY;
            _cameraTransform = Camera.main.transform;
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target  != null)
            {
                _cameraTransform.position = _target.position + -_cameraTransform.forward * _offsetY;                
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