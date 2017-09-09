using UnityEngine;

namespace Control.Actions
{
    [CreateAssetMenu(fileName = "HammerConfig", menuName = "Game/HammerConfig")]
    public class HammerConfig : ScriptableObject
    {
        [SerializeField]
        public float Velocity;
        [SerializeField]
        public float Range;
        [SerializeField]
        public GameObject HammerPrefab;

        [SerializeField]
        public float CooldownTimeInSeconds;
    }
}