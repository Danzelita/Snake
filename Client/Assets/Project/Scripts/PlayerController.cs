using System.Collections.Generic;
using Colyseus.Schema;
using Project.Scripts.Multiplayer;
using Project.Scripts.Multiplayer.Generated;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] public float _cameraOffsetY = 16f;
        [SerializeField] private Transform _cursor;
        private Snake _snake;
        private Camera _camera;
        private Plane _plane;
        private MultiplayerManager _multiplayerManager;
        
        private readonly Dictionary<string, object> _data = new();
        private PlayerAim _playerAim;
        private Player _player;

        public void Init(PlayerAim aim, Player player, Snake snake, MultiplayerManager multiplayerManager)
        {
            _playerAim = aim;
            _player = player;
            _snake = snake;
            _multiplayerManager = multiplayerManager;
            
            _snake.AddComponent<CameraManager>().Init(_cameraOffsetY);
            
            _camera = Camera.main;
            _plane = new Plane(Vector3.up, Vector3.zero);
            
            _player.OnChange += OnChange;
        }
        private void OnChange(List<DataChange> changes)
        {
            Vector3 position = _snake.transform.position;
            foreach (DataChange change in changes)
            {
                switch (change.Field)
                {
                    case "x":
                        position.x = (float)change.Value;
                        break;
                    case "z":
                        position.z = (float)change.Value;
                        break;
                    case "d":
                        _snake.SetDetailCount((byte)change.Value);
                        break;
                    default:
                        Debug.Log("Unknown field: " + change.Field);
                        break;
                }
            }
            _snake.SetRotation(position);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveCursor();
                _playerAim.SetTargetDirection(_cursor.position);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                _multiplayerManager.LeaveRoom();
            }
            
            SendMove();
        }

        private void MoveCursor()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out float distance))
            {
                _cursor.position = ray.GetPoint(distance);
            }
        }

        private void SendMove()
        {
            _playerAim.GetMoveInfo(out Vector3 position);

            _data["x"] = position.x;
            _data["z"] = position.z;
            
            _multiplayerManager.SendToServer("move", _data);
        }
    }
}
