using UnityEngine;

namespace GameLogic
{
    public class PlayerIdConfig : ScriptableObject
    {
        [SerializeField] private Sprite[] _playerIdSprites = new Sprite[4];

        public Sprite GetSpriteForPlayer(int playerId)
        {
            return _playerIdSprites[playerId - 1];
        }
    }
}