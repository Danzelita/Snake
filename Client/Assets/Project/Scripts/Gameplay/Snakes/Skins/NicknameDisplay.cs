using TMPro;
using UnityEngine;

namespace Project.Scripts.Gameplay.Snakes.Skins
{
    public class NicknameDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nickText;

        public void SetNickname(string nickname) => 
            _nickText.text = nickname;
    }
}