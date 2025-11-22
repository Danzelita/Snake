using System;
using Project.Scripts.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class StarterPopup : Popup
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _joinButton;
        private string _inputName;

        private void OnEnable()
        {
            _joinButton.onClick.AddListener(OnJoinButtonClick);
            _inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }

        private void OnDisable()
        {
            _joinButton.onClick.RemoveListener(OnJoinButtonClick);
            _inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
        }

        private void OnInputFieldValueChanged(string input)
        {
            _inputName = input;
        }

        private void OnJoinButtonClick()
        {
            MultiplayerManager.Instance.Join(_inputName);
        }
    }
}