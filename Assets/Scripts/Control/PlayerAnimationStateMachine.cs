using UniRx;

namespace Control
{
    public enum PlayerAnimationState
    {
        Idle = 0,
        Walk = 1,
        Jump = 2,
        Attack = 3,
        Skill = 4,
        Death = 5
    }

    public class PlayerAnimationStateMachine
    {
        public ReactiveProperty<PlayerAnimationState> State = new ReactiveProperty<PlayerAnimationState>(PlayerAnimationState.Idle);

        public void TriggerIdle()
        {
            State.Value = PlayerAnimationState.Idle;
        }

        public void TriggerWalking()
        {
            State.Value = PlayerAnimationState.Walk;
        }
    }
}