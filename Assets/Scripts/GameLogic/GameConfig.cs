using Entities;
using UnityEngine;

namespace LifeSystem
{
    public class GameConfig : ScriptableObject
    {
        public int AmountOfPlayers = 2;
        public int[] CharactersSelected = new int[4];
        public Player[] PlayerPrefab;

        public TeamPointSystem TeamPointSystemPrefab;
    }
}