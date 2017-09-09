using UnityEngine;
using UniRx;

namespace Control
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private PlayerControllerBase _controller;
        [SerializeField] private Animator _animator;

        private PlayerAnimationStateMachine _stateMachine = new PlayerAnimationStateMachine();

        private const float MinWalkingVelocity = 0.01F;

        private void Start()
        {
            _stateMachine.State.Subscribe(state =>
            {
                Debug.Log("New state: " + state);
                _animator.SetInteger("State", (int) state);
            });
        }

        private void Update()
        {
            if (CouldTriggerIdle()) _stateMachine.TriggerIdle();
            if (CouldTriggerWalking()) _stateMachine.TriggerWalking();
        }

        private bool CouldTriggerIdle()
        {
            return _stateMachine.State.Value != PlayerAnimationState.Idle 
                && Mathf.Abs(_controller.InputProvider.GetAxis("Horizontal")) < MinWalkingVelocity;
        }

        private bool CouldTriggerWalking()
        {
            return _stateMachine.State.Value != PlayerAnimationState.Walk 
                && Mathf.Abs(_controller.InputProvider.GetAxis("Horizontal")) > MinWalkingVelocity;
        }



    }
}