using Assets.Scripts.Entities;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace LifeSystem
{
    public class GameInit : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private LevelConfig _levelConfig;

        private void Start()
        {
            var crossLevelDataTransfer = FindObjectOfType<CrossLevelDataTransfer>();
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
            player.Color = Random.ColorHSV();
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