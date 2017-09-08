using UnityEngine;

namespace Control
{
    class UnityInputProvider : IInputProvider
    {
        public float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return Input.GetButtonDown(buttonName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return Input.GetButtonUp(buttonName);
        }
    }
}