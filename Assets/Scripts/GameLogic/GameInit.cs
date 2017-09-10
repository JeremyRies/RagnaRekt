using System.Runtime.InteropServices;
using Assets.Scripts.Entities;
using Entities;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace LifeSystem
{
    public class GameInit : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private LevelConfig _levelConfig;

        private TeamPointSystem _teamPointSystem;

        private Team _team1;
        private Team _team2;

        private void Start()
        {
            _team1 = new Team(1);
            _team2 = new Team(2);

            var crossLevelDataTransfer = FindObjectOfType<CrossLevelDataTransfer>();
            _teamPointSystem = Instantiate(_gameConfig.TeamPointSystemPrefab);

            _teamPointSystem.AddTeams(_team1, _team2);

            if (crossLevelDataTransfer != null)
            {
                _gameConfig.CharactersSelected = crossLevelDataTransfer.GetSelectedCharacters();
            }
            Observable.Range(1, _gameConfig.AmountOfPlayers).Subscribe(CreatePlayer);


        }

        private void CreatePlayer(int playerId)
        {
            var character = _gameConfig.CharactersSelected[playerId - 1];
            var player = Instantiate(_gameConfig.PlayerPrefab[character - 1]);
            player.PlayerId = playerId;
            if (playerId%2 == 0)
            {
                _team2.AddPlayer(player);
            }
            else
            {
                _team1.AddPlayer(player);
            }
            // player.Color = Random.ColorHSV();
            player.TeamPointSystem = _teamPointSystem;
            PositionRandomly(playerId, player);
        }

        private void PositionRandomly(int playerId, Player player)
        {
            var maxLeft = playerId % 2 == 0 ? 0 : _levelConfig.LevelLeftMaxPosition;
            var maxRight = playerId % 2 == 0 ? _levelConfig.LevelRightMaxPosition : 0;
            var xpos = Random.Range(maxLeft, maxRight);
            var pos = new Vector2(xpos, _levelConfig.LevelYMaxPosition);
            player.transform.position = pos;
            if (pos.x > 0) player.LookLeft();
        }
    }
}