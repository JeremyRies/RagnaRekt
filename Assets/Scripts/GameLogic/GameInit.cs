using Control;
using Control.Airconsole;
using Entities;
using LifeSystem;
using NDream.AirConsole;
using Sound;
using UniRx;
using UnityEngine;

namespace GameLogic
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
                _gameConfig.AmountOfPlayers = crossLevelDataTransfer.GetNumberPlayers();
            }

            Observable.Range(1, _gameConfig.AmountOfPlayers).Subscribe(CreatePlayer);

            BackgroundMusic.BackgroundMusicInstance.SetClip(ClipIdentifier.BackgroundGame);
            BackgroundMusic.BackgroundMusicInstance.StartPlay();

        }


        private void CreatePlayer(int playerId)
        {
            var character = _gameConfig.CharactersSelected[playerId - 1];
            var player = Instantiate(_gameConfig.PlayerPrefab[character - 1]);
            player.PlayerId = playerId;
            player.HeroType = (HeroType) character;
            HandleInputSelection(player);

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

        private void HandleInputSelection(Player player)
        {
            if (_gameConfig.UseAirconsole)
            {
                var inputProviderObject = new GameObject("Airconsole InputProvider");
                var airconsoleInputProvider = inputProviderObject.AddComponent<AirconsoleInputProvider>();
                airconsoleInputProvider.Initialize(player.PlayerId);

                player.InputProvider = airconsoleInputProvider;
            }
            else
            {
                player.InputProvider = new UnityInputProvider(player.PlayerId);
            }
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