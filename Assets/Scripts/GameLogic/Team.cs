using System.Collections.Generic;
using Entities;

namespace GameLogic
{
    public class Team
    {
        public int TeamId { get { return _teamId; } }

        private List<Player> _players = new List<Player>();
        private int _teamId;

        public Team(int i)
        {
            _teamId = i;
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
            player.Team = this;
        }

        public List<Player> GetPlayers()
        {
            return _players;
        }
    }
}
