using Entities;

namespace Control
{
    class UnityPlayerController : PlayerControllerBase
    {
        protected override IInputProvider GetInputProvider(int playerId)
        {
            return new UnityInputProvider(playerId);
        }
    }
}