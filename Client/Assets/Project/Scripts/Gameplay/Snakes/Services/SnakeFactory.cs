using Project.Scripts.Gameplay.Controller;
using Project.Scripts.Gameplay.Snakes.Core;
using Project.Scripts.Gameplay.Snakes.Network;
using Project.Scripts.Logic;
using Project.Scripts.Multiplayer;
using Project.Scripts.Multiplayer.Generated;
using Project.Scripts.Settings;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Services
{
    public class SnakeFactory
    {
        private readonly CameraManager _cameraManager;
        private readonly Snake _snakePrefab;
        private readonly PlayerController _playerControllerPrefab;
        private readonly PlayerAim _playerAimPrefab;
        private readonly GameSettings _gameSettings;

        public SnakeFactory(
            CameraManager cameraManager,
            Snake snakePrefab,
            PlayerController playerControllerPrefab,
            PlayerAim playerAimPrefab,
            GameSettings gameSettings
            )
        {
            _cameraManager = cameraManager;
            _snakePrefab = snakePrefab;
            _playerControllerPrefab = playerControllerPrefab;
            _playerAimPrefab = playerAimPrefab;
            _gameSettings = gameSettings;
        }

        public SnakeNetworkController CreatePlayer(
            MultiplayerManager multiplayerManager,
            Player player,
            Vector3 spawnPosition,
            Quaternion rotation,
            out PlayerController playerController
            )
        {
            Snake newSnake = CreateSnake(player, spawnPosition, rotation);
            
            PlayerController newPlayerController = Object.Instantiate(_playerControllerPrefab);
            PlayerAim newPlayerAim = Object.Instantiate(_playerAimPrefab, spawnPosition, rotation);

            newPlayerController.Init(newPlayerAim, multiplayerManager);
            newPlayerAim.Init(newSnake.Speed);

            SnakeNetworkController snakeNetworkController = new(newSnake, player);
            
            _cameraManager.SetTarget(newSnake.transform, newPlayerController.CameraOffsetY);
            
            playerController = newPlayerController;

            return snakeNetworkController;
        }

        public SnakeNetworkController CreateEnemy(Player player, Vector3 spawnPosition, Quaternion rotation)
        {
            Snake newSnake = CreateSnake(player, spawnPosition, rotation);
            
            SnakeNetworkController snakeNetworkController = new(newSnake, player);
            
            return snakeNetworkController;
        }

        private Snake CreateSnake(Player player, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            Snake snakePrefab = _snakePrefab;
            Snake newSnake = Object.Instantiate(snakePrefab, spawnPosition, spawnRotation);
            newSnake.Init(_gameSettings.SkinsSettings.Skins[player.skin],player.d);
            
            return newSnake;
        }
    }
}