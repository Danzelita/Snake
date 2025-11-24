using System.Collections.Generic;
using Project.Scripts.Gameplay.Snakes.Core;
using Project.Scripts.Multiplayer;
using UnityEngine;

namespace Project.Scripts.Gameplay.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] public float CameraOffsetY = 16f;
        [SerializeField] private Transform _cursor;
        [SerializeField] private PlayerInteraction playerInteraction;
        private Camera _camera;
        private Plane _plane;
        private MultiplayerManager _multiplayerManager;
        
        private readonly Dictionary<string, object> _data = new();
        private PlayerAim _playerAim;
        private Snake _snake;

        public void Init(PlayerAim aim, Snake snake, MultiplayerManager multiplayerManager)
        {
            _playerAim = aim;
            _multiplayerManager = multiplayerManager;
            _snake = snake;
            
            _camera = Camera.main;
            _plane = new Plane(Vector3.up, Vector3.zero);
            playerInteraction.Init(_snake.transform, multiplayerManager.SnakeService);
        }
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveCursor();
                _playerAim.SetTargetDirection(_cursor.position);
            }

            //if (Input.GetKey(KeyCode.Space))
            //{
            //    
            //}
            
            SendMove();
        }

        private void MoveCursor()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out float distance)) 
                _cursor.position = ray.GetPoint(distance);
        }

        private void SendMove()
        {
            _playerAim.GetMoveInfo(out Vector3 position);

            _data["x"] = position.x;
            _data["z"] = position.z;
            
            _multiplayerManager.SendToServer("move", _data);
        }

        public void Destroy()
        {
            _playerAim.Destroy();
            Destroy(gameObject);
        }
    }
}
