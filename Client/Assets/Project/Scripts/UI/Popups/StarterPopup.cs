using Project.Scripts.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Popups
{
    public class StarterPopup : Popup
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _joinButton;
        private string _inputName;

        private const string Allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGIJKLMNPQRSTUVWXYZ0123456789_-";

        private void OnEnable()
        {
            _joinButton.onClick.AddListener(OnJoinButtonClick);
            _inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            _inputField.onValidateInput += OnValidateInput;
        }

        private char OnValidateInput(string text, int charIndex, char addedChar)
        {
            if (Allowed.Contains(addedChar))
                return addedChar;
            return '0';
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
            Close(delayClose: 0.1f);
        }
    }
}