using UnityEngine;

namespace Control
{
    [CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Game/PlayerMovementConfig")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [SerializeField]
        public float
            MaxJumpHeight = 4,
            MinJumpHeight = 1,
            MoveSpeed = 6,
            TimeToJumpApex = .4f;
    }
}