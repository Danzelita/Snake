using UnityEngine;

namespace Project.Scripts.Logic
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Start() => 
            _cameraTransform = Camera.main.transform;

        private void LateUpdate() => 
            transform.LookAt(_cameraTransform);
    }
}