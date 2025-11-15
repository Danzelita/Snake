using UnityEngine;

namespace Project.Scripts
{
    public class CameraManager : MonoBehaviour
    {
        public void Init(float offsetY)
        {
            Transform cameraTransform = Camera.main.transform;

            cameraTransform.parent = transform;
            cameraTransform.localPosition = Vector3.up  * offsetY;
        }

        private void OnDestroy()
        {
            Camera camera = Camera.main;

            if (camera == null) return;
            
            camera.transform.parent = null;
        }
    }
}