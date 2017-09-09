using UnityEngine;

namespace Control.Actions
{
    public abstract class Action : MonoBehaviour
    {
        public abstract void TryToActivate(Direction direction);
    }
}