using System;
using Project.Scripts.Data;
using Project.Scripts.Gameplay.Snakes.Animation;
using Project.Scripts.Gameplay.Snakes.Skins;
using Project.Scripts.Multiplayer;
using Project.Scripts.Settings.Skins;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Core
{
    public class Snake : MonoBehaviour
    {
        private const string PlayerLayer = "Player";
        public float Speed => _speed;

        [SerializeField] private SnakeTrail _trailPrefab;
        [SerializeField] private Transform _head;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _rotateSpeed = 90f;
        private SnakeTrail _trail;
        private string _sessionId;
        public Action OnCollect;

        public void Init(SkinSettings skinSettings, int detailCount, string sessionId, bool isPlayer = false)
        {
            _sessionId = sessionId;
            _trail = Instantiate(_trailPrefab, _head.position, Quaternion.identity);
            if (isPlayer)
            {
                int layer = LayerMask.NameToLayer(PlayerLayer);
                gameObject.layer = layer;

                for (int i = 0; i < gameObject.transform.childCount; i++)
                    gameObject.transform.GetChild(i).gameObject.layer = layer;
            }

            _trail.Init(_head, skinSettings, detailCount, isPlayer, LayerMask.NameToLayer(PlayerLayer));

            GetComponent<SkinDisplay>().SetSkin(skinSettings);
            GetComponent<SnakeEffects>().Init(skinSettings);
        }

        public void SetDetailCount(byte detailCount) =>
            _trail.SetDetailCount(detailCount);

        private void Update() =>
            Move();

        public void Destroy()
        {
            DetailPositionsData detailPositionsData = _trail.GetDetailPositions();
            detailPositionsData.Id = _sessionId;
            string json = JsonUtility.ToJson(detailPositionsData);
            MultiplayerManager.Instance.SendToServer("gameOver", json);
            Debug.Log("Game Over");

            Destroy(gameObject);
            _trail.Destroy();
        }

        public void SetRotation(Vector3 point)
        {
            Vector3 toPoint = point - _head.position;
            Quaternion.LookRotation(toPoint, Vector3.up);

            _head.rotation = Quaternion.LookRotation(toPoint, Vector3.up);
        }

        private void Move() =>
            transform.position += _head.forward * Time.deltaTime * _speed;

        public void Collect() => 
            OnCollect?.Invoke();
    }
}