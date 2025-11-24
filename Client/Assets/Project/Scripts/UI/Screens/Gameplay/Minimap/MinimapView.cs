using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.Snakes.Services;
using Project.Scripts.Multiplayer;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.UI.Screens.Gameplay.Minimap
{
    public class MinimapView : MonoBehaviour
    {
        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _enemyColor;
        [SerializeField] private RectTransform _minimapContainer;
        [SerializeField] private Marker _markerPrefab;

        private readonly Dictionary<Player, Marker> _markers = new();

        public void Init(SnakeService snakeService)
        {
            foreach (Player player in snakeService.Snakes)
                CreateMarker(player);

            snakeService.OnCreateSnake += CreateMarker;
            snakeService.OnRemoveSnake += RemoveMarker;
        }

        private void CreateMarker(Player player)
        {
            if (_markers.ContainsKey(player))
                return;
            
            PlayerMarker playerMarker = new PlayerMarker(player);
            
            Marker marker = Instantiate(_markerPrefab, _minimapContainer);
            marker.Init(MultiplayerManager.Instance.SessionId == player.sessionId ? _playerColor : _enemyColor);
            
            playerMarker.OnRectPositionChange += (rect) =>
            {
                Vector2 markerPosition = new Vector2(
                    _minimapContainer.sizeDelta.x * rect.x,
                    _minimapContainer.sizeDelta.y * rect.y
                );
                marker.SetPosition(markerPosition);
            };
            
            _markers.Add(player, marker);
        }

        private void RemoveMarker(Player player)
        {
            if (_markers.Remove(player, out Marker marker)) 
                Destroy(marker.gameObject);
        }
    }
}