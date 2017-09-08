namespace Control
{
    class UnityPlayerController : PlayerControllerBase
    {
        protected override IInputProvider GetInputProvider()
        {
            return new UnityInputProvider();
        }
    }
}