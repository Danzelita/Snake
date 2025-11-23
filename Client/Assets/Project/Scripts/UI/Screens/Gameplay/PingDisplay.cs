using System;
using Project.Scripts.Multiplayer;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Screens.Gameplay
{
    public class PingDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _pingText;
        private MultiplayerManager _multiplayerManager;

        public void Init(MultiplayerManager multiplayerManager)
        {
            _multiplayerManager = multiplayerManager;
            
            _multiplayerManager.OnPingChange += OnPingChane;
        }


        private void OnPingChane(float ping)
        {
            float displayPing = ping * 1000f;
            string pingText = displayPing.ToString("0");
            _pingText.text = $"Ping: {pingText}";
        }

        private void OnDestroy() => 
            _multiplayerManager.OnPingChange -= OnPingChane;
    }
}