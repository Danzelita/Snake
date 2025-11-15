using System.Collections.Generic;
using Colyseus;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        #region Server

        private const string GameRoomName = "state_handler";

        private ColyseusRoom<State> _room;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            _room = await client.JoinOrCreate<State>(GameRoomName);

            _room.OnStateChange += RoomOnStateChange;
        }

        private void RoomOnStateChange(State state, bool isFirstState)
        {
            if (isFirstState == false) return;

            _room.OnStateChange -= RoomOnStateChange;
            state.players.ForEach((key, player) =>
            {
                if (key == _room.SessionId)
                    CreatePlayer(key, player);
                else
                    CreateEnemy(key, player);
            });

            _room.State.players.OnAdd += CreateEnemy;
            _room.State.players.OnRemove += RemoveEnemy;
        }

        public void SendToServer(string key, Dictionary<string, object> data) =>
            _room.Send(key, data);

        #endregion


        #region Player

        [SerializeField] private PlayerAim _playerAimPrefab;
        [SerializeField] private Snake _snakePrefab;
        [SerializeField] private PlayerController playerControllerPrefab;

        private void CreatePlayer(string key, Player player)
        {
            Vector3 position = new Vector3(player.x, 0f, player.z);
            Quaternion rotation = Quaternion.identity;
            
            Snake newSnake = Instantiate(_snakePrefab, position, rotation);
            newSnake.Init(player.d);
            
            PlayerAim playerAim = Instantiate(_playerAimPrefab, position, rotation);
            playerAim.Init(_snakePrefab.Speed);

            PlayerController newPlayerController = Instantiate(playerControllerPrefab);
            newPlayerController.Init(playerAim, player, newSnake, this);
        }

        #endregion

        #region Enemy

        private readonly Dictionary<string, EnemyController> _enemies = new();

        private void CreateEnemy(string key, Player player)
        {
            Vector3 position = new Vector3(player.x, 0f, player.z);
            Snake newSnake = Instantiate(_snakePrefab, position, Quaternion.identity);
            newSnake.Init(player.d);

            var enemyController = new EnemyController(newSnake, player);

            _enemies.Add(key, enemyController);
        }

        private void RemoveEnemy(string key, Player value)
        {
            if (_enemies.Remove(key, out EnemyController enemyController) == false) 
                return;
            
            enemyController.Dispose();
        }

        #endregion

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            LeaveRoom();
        }

        public void LeaveRoom() =>
            _room?.Leave();
    }
}