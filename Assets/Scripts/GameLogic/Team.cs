using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

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
    }

    public List<Player> GetPlayers()
    {
        return _players;
    }
}
