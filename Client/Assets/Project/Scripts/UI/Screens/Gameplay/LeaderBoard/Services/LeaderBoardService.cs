using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.UI.Screens.Gameplay.LeaderBoard.Services
{
    public class UsernameScorePair
    {
        public string Username { get; set; }
        public float Score { get; set; }
    }
    public class LeaderBoardService
    {
        public void Init(MapSchema<Player> statePlayers)
        {
            statePlayers.ForEach(AddLeader);
            statePlayers.OnAdd += AddLeader;
            statePlayers.OnRemove += RemoveLeader;
        }
        
        public List<UsernameScorePair> Leaderboard = new();
        
        private Dictionary<string, Player> _leaders = new();

        private void AddLeader(string sessionID, Player player)
        {
            if (_leaders.ContainsKey(sessionID))
                return;

            _leaders.Add(sessionID, player);
        }

        private void RemoveLeader(string sessionID, Player player) => 
            _leaders.Remove(sessionID);

        public void UpdateBoard()
        {
            int topCount = Mathf.Clamp(_leaders.Count, 0, 5);
            var top = _leaders
                .OrderByDescending(pair => pair.Value.score)
                .Take(topCount)
                .ToList();
            
            Leaderboard.Clear();
            foreach (var pair in top)
            {
                Leaderboard.Add(new UsernameScorePair()
                {
                    Username = pair.Value.name,
                    Score = pair.Value.score
                });
            }
        }
    }
}