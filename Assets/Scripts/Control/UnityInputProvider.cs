using UnityEngine;

namespace Control
{
    class UnityInputProvider : IInputProvider
    {
        private readonly int _playerId;

        public UnityInputProvider(int playerId)
        {
            _playerId = playerId;
        }

        public float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName + " " + _playerId);
        }

        public bool GetButtonDown(string buttonName)
        {
            return Input.GetButtonDown(buttonName + " " + _playerId);
        }

        public bool GetButtonUp(string buttonName)
        {
            return Input.GetButtonUp(buttonName + " " + _playerId);
        }
    }
}