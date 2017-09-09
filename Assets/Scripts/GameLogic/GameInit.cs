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
            Observable.Range(1, _gameConfig.AmountOfPlayers).Subscribe(CreatePlayer);
        }

        private void CreatePlayer(int playerId)
        {
            var player = Instantiate(_gameConfig.PlayerPrefab);
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