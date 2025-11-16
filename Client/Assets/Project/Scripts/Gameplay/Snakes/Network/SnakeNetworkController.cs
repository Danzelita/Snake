using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Project.Scripts.Gameplay.Snakes.Core;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Network
{
    public class SnakeNetworkController : IDisposable
    {
        private readonly Player _player;
        private readonly Snake _snake;

        public SnakeNetworkController(Snake snake, Player player)
        {
            _snake = snake;
            _player = player;
            
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

        public void Dispose()
        {
            _player.OnChange -= OnChange;
            
            _snake.Destroy();
        }
    }
}