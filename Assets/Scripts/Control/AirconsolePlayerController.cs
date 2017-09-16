using Airconsole;
using UnityEngine;

namespace Control
{
    class AirconsolePlayerController : PlayerControllerBase
    {
        [SerializeField] private AirconsoleInputProvider _airconsoleInputProvider;
        protected override IInputProvider GetInputProvider(int playerPlayerId)
        {
            return _airconsoleInputProvider;
        }
    }
}