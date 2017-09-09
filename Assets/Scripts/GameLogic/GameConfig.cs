using Assets.Scripts.Entities;
using Control;
using UnityEngine;

namespace LifeSystem
{
    public class GameConfig : ScriptableObject
    {
        public int AmountOfPlayers = 2;
        public Player PlayerPrefab;
    }
}