using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using Project.Scripts.Gameplay.Controller;
using Project.Scripts.Gameplay.Foods.Services;
using Project.Scripts.Gameplay.Snakes.Core;
using Project.Scripts.Gameplay.Snakes.Services;
using Project.Scripts.Logic;
using Project.Scripts.Multiplayer.Generated;
using Project.Scripts.Settings;
using Project.Scripts.UI;
using Project.Scripts.UI.Screens.Gameplay.LeaderBoard.Services;
using UnityEngine;

namespace Project.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        public float Ping { get; set; }
        public Action<float> OnPingChange;
        public string SessionId => _room.SessionId;
        public SnakeService SnakeService => _snakeService;
        public LeaderBoardService LeaderBoardService => _leaderBoardService;

        [SerializeField] private UIRoot _uiRoot;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private Snake _snakePrefab;
        [SerializeField] private PlayerController _playerControllerPrefab;
        [SerializeField] private PlayerAim _playerAimPrefab;

        private const string GameRoomName = "state_handler";

        private ColyseusRoom<State> _room;
        private SettingsProvider _settingsProvider;
        private SnakeService _snakeService;
        private FoodService _foodService;
        private LeaderBoardService _leaderBoardService;

        private float _lastPingSendTime;
        
        protected override void Awake()
        {
            base.Awake();
            
            _uiRoot.ShowLoadingScreen();

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
            
            _uiRoot.HideLoadingScreen();
            _uiRoot.OpenStarterPopup();
        }

        private void Update()
        {
            if (_room == null) 
                return;
            if(Time.time - _lastPingSendTime < 1f)
                return;

            _lastPingSendTime = Time.time;

            float localTime = Time.realtimeSinceStartup;
            SendToServer("ping", localTime.ToString());
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

            _foodService = new FoodService(this, _settingsProvider.GameSettings.FoodsSettings);
            _foodService.Init(state.foods);

            _leaderBoardService = new LeaderBoardService();
            _leaderBoardService.Init(state.players);
            
            
            _room.OnMessage<string>("pong", (data) =>
            {
                float time = float.Parse(data);
                Ping = Time.realtimeSinceStartup - time;
                OnPingChange?.Invoke(Ping);
            });

            _room.OnStateChange -= RoomOnStateChange;
        }

        public void SendToServer(string key, Dictionary<string, object> data) =>
            _room.Send(key, data);

        public void SendToServer(string key, string data) =>
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
            _snakeService.Dispose();
            _foodService.Dispose();
        }

        public void Join(string inputName)
        {
            SendToServer("join", inputName);
            _uiRoot.OpenGameplayScreen();
        }

        public void Join(string playerName, float delay) => 
            StartCoroutine(DelayJoin(playerName, delay));

        private IEnumerator DelayJoin(string playerName, float delay)
        {
            yield return new WaitForSeconds(delay);
            Join(playerName);
        }
    }
}