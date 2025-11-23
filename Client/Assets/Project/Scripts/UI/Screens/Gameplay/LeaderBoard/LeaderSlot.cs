using Project.Scripts.UI.Screens.Gameplay.LeaderBoard.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screens.Gameplay.LeaderBoard
{
    public class LeaderSlot : MonoBehaviour
    {
        [SerializeField] private Image _coloredImage;
        [SerializeField] private Image _coloredGradient;
        [SerializeField] private TextMeshProUGUI _leaderName;
        [SerializeField] private TextMeshProUGUI _leaderScore;
        [SerializeField] private TextMeshProUGUI _index;
        [SerializeField] private Color[] _colorsByIndex;
        public void SetLeader(UsernameScorePair usernameScorePair, int index)
        {
            Color color = _colorsByIndex[Mathf.Clamp(index, 0, _colorsByIndex.Length - 1)];
            
            _coloredImage.color = color;
            _coloredGradient.color = new Color(color.r, color.g, color.b, color.a / 2f);
            _leaderName.text = usernameScorePair.Username;
            _leaderScore.text = usernameScorePair.Score.ToString();
            _index.text = $"{index + 1}";
        }
    }
}