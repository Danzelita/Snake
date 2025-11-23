using Project.Scripts.UI.Screens.Gameplay.LeaderBoard.Services;
using UnityEngine;

namespace Project.Scripts.UI.Screens.Gameplay.LeaderBoard
{
    public class LeaderBoardView : MonoBehaviour
    {
        [SerializeField] private LeaderSlot[] _leaderSlots;
        
        private LeaderBoardService _leaderBoardService;

        public void Init(LeaderBoardService leaderBoardService)
        {
            _leaderBoardService = leaderBoardService;
        }

        private void Update()
        {
            if (_leaderBoardService == null)
                return;
            
            _leaderBoardService.UpdateBoard();
            UpdateBoard();
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < _leaderSlots.Length; i++) 
                _leaderSlots[i].gameObject.SetActive(i < _leaderBoardService.Leaderboard.Count);

            for (int i = 0; i < _leaderBoardService.Leaderboard.Count; i++)
            {
                _leaderSlots[i].SetLeader(_leaderBoardService.Leaderboard[i], i);
            }
        }
    }
}