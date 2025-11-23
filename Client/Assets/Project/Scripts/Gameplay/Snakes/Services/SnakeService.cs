using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Project.Scripts.Gameplay.Controller;
using Project.Scripts.Gameplay.Snakes.Network;
using Project.Scripts.Multiplayer;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Services
{
    public class SnakeService : IDisposable
    {
        private readonly MultiplayerManager _multiplayerManager;
        private readonly SnakeFactory _snakeFactory;

        private SnakeNetworkController _playerSnakeNetworkController;
        private readonly Dictionary<string, SnakeNetworkController> _enemies = new();
        private PlayerController _playerController;

        public SnakeService(MultiplayerManager multiplayerManager, SnakeFactory snakeFactory)
        {
            _multiplayerManager = multiplayerManager;
            _snakeFactory = snakeFactory;
        }

        public void Init(MapSchema<Player> statePlayers)
        {
            statePlayers.ForEach((key, player) =>
            {
                if (key == _multiplayerManager.SessionId)
                    CreatePlayer(key, player);
                else
                    CreateEnemy(key, player);
            });

            statePlayers.OnAdd += (key, player) =>
            {
                if (key == _multiplayerManager.SessionId)
                    CreatePlayer(key, player);
                else
                    CreateEnemy(key, player);
            };
            statePlayers.OnRemove += RemoveEnemy;
        }

        private void CreatePlayer(string key, Player player)
        {
            Vector3 spawnPosition = GetSnakeSpawnPosition(player);
            Quaternion rotation = Quaternion.identity;
            
            _playerSnakeNetworkController = _snakeFactory.CreatePlayer(
                _multiplayerManager,
                player,
                key,
                spawnPosition,
                rotation,
                out PlayerController playerController
                );
            _playerController = playerController;
        }

        private void CreateEnemy(string key, Player player)
        {
            Vector3 spawnPosition = GetSnakeSpawnPosition(player);
            Quaternion rotation = Quaternion.identity;
            
            SnakeNetworkController networkController = _snakeFactory.CreateEnemy(player, key, spawnPosition, rotation);
            _enemies.Add(key, networkController);
        }

        private void RemoveEnemy(string key, Player value)
        {
            if (_enemies.Remove(key, out SnakeNetworkController networkController) == false)
                return;

            networkController.Dispose();
        }

        public void Dispose()
        {
            foreach (SnakeNetworkController networkController in _enemies.Values) 
                networkController.Dispose();
            
            _playerSnakeNetworkController?.Dispose();
            _playerController?.Destroy();
            
            _enemies.Clear();
        }

        public void DestroyPlayer()
        {
            _multiplayerManager.Join(_playerSnakeNetworkController.Player.name, delay: 2f);
            
            _playerSnakeNetworkController?.Dispose();
            _playerController?.Destroy();
            
            _playerSnakeNetworkController = null;
            _playerController = null;
        }

        private Vector3 GetSnakeSpawnPosition(Player player) =>
            new(player.x, 0f, player.z);
    }
}