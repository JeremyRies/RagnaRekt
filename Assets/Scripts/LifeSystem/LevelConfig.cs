using UnityEngine;

namespace LifeSystem
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField]
        public float LevelLeftMaxPosition;
        [SerializeField]
        public float LevelRightMaxPosition;
        [SerializeField]
        public float LevelYMaxPosition;

        [SerializeField]
        public float LevelYDeathPosition;
    }
}