using Project.Scripts.Multiplayer;
using Project.Scripts.UI.Screens.Gameplay.LeaderBoard;
using UnityEngine;

namespace Project.Scripts.UI.Screens.Gameplay
{
    public class GameplayScreen : Screen
    {
        [SerializeField] private LeaderBoardView _leaderBoardView;
        [SerializeField] private PingDisplay _pingDisplay;

        protected override void OnOpen()
        {
            base.OnOpen();
            
            _leaderBoardView.Init(MultiplayerManager.Instance.LeaderBoardService);
            _pingDisplay.Init(MultiplayerManager.Instance);
        }
    }
}