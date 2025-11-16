using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using Project.Scripts.Gameplay.Controller;
using Project.Scripts.Gameplay.Snakes.Core;
using Project.Scripts.Gameplay.Snakes.Services;
using Project.Scripts.Logic;
using Project.Scripts.Multiplayer.Generated;
using Project.Scripts.Settings;
using UnityEngine;

namespace Project.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        public string SessionId => _room.SessionId;

        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private Snake _snakePrefab;
        [SerializeField] private PlayerController _playerControllerPrefab;
        [SerializeField] private PlayerAim _playerAimPrefab;

        private const string GameRoomName = "state_handler";

        private ColyseusRoom<State> _room;
        private SnakeService _snakeService;
        private SettingsProvider _settingsProvider;


        protected override void Awake()
        {
            base.Awake();
            
            _settingsProvider = new SettingsProvider();
            _settingsProvider.LoadGameSettings();

            DontDestroyOnLoad(gameObject);

            InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            int skinCount = _settingsProvider.GameSettings.SkinsSettings.Skins.Length;

            Dictionary<string, object> data = new()
            {
                ["skins"] = skinCount,
            };
            
            _room = await client.JoinOrCreate<State>(GameRoomName, data);

            _room.OnStateChange += RoomOnStateChange;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J)) 
                Connect();

            if (Input.GetKeyDown(KeyCode.L)) 
                LeaveRoom();
        }

        private void RoomOnStateChange(State state, bool isFirstState)
        {
            if (isFirstState == false) return;

            SnakeFactory snakeFactory = new SnakeFactory(
                _cameraManager,
                _snakePrefab,
                _playerControllerPrefab,
                _playerAimPrefab,
                _settingsProvider.GameSettings
                );
            
            _snakeService = new SnakeService(this, snakeFactory);
            _snakeService.Init(state.players);


            _room.OnStateChange -= RoomOnStateChange;
        }

        public void SendToServer(string key, Dictionary<string, object> data) => 
            _room.Send(key, data);

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            LeaveRoom();
        }

        public void LeaveRoom()
        {
            if (_room == null)
                return;
            
            _room.Leave();
            _snakeService?.Dispose();
        }
    }
}