using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.UI.Screens.Gameplay.Minimap
{
    public class PlayerMarker : IDisposable
    {
        public Vector2 RectPosition { get; private set; }
        public Action<Vector2> OnRectPositionChange;

        private Vector2 _position;
        private readonly Player _player;


        public PlayerMarker(Player player)
        {
            _player = player;
            
            _player.OnChange += PlayerOnOnChange;
        }

        private void PlayerOnOnChange(List<DataChange> changes)
        {
            foreach (var change in changes)
            {
                switch (change.Field)
                {
                    case "x":
                        _position.x = (float)change.Value;
                        RectPositionChange();
                        break;
                    case "z":
                        _position.y = (float)change.Value;
                        RectPositionChange();
                        break;
                }
            }
        }

        private void RectPositionChange()
        {
            Vector2 rectPosition = _position / 256f;
            
            RectPosition = rectPosition;
            OnRectPositionChange?.Invoke(rectPosition);
        }

        public void Dispose()
        {
            _player.OnChange -= PlayerOnOnChange;
        }
    }
}