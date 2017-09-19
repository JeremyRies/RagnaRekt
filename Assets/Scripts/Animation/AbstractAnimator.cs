using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Animation {

    public abstract class AbstractAnimator<State, Property>
    {
        private IDictionary<State, IAnimationConfig<Property>> _dictionary;
        private State _defaultAnimation;
        private IDisposable _currentAnimation;

        private readonly ReactiveProperty<State> _currentAnimationState;
        public IObservable<State> ShownAnimation { get { return _currentAnimationState; } }
        public State CurrentAnimation { get { return _currentAnimationState.Value; } }

        protected AbstractAnimator(IAnimatorConfig<State, Property> config)
        {
            _dictionary = CreateDictionary(config.AnimationSteps);
            _defaultAnimation = config.DefaultAnimation;

            _currentAnimationState = new ReactiveProperty<State>(_defaultAnimation);
        }

        public IAnimatorConfig<State, Property> Config
        {
            set
            {
                _defaultAnimation = value.DefaultAnimation;
                _dictionary = CreateDictionary(value.AnimationSteps);
            }
        }

        private static IDictionary<State, IAnimationConfig<Property>> CreateDictionary(List<AnimationStep<State, Property>> steps)
        {
            var result = new Dictionary<State, IAnimationConfig<Property>>();
            steps.ForEach(step => result.Add(step.Name, step.Animation));
            return result;
        }

        public void PlayAnimation(State state)
        {
            InterruptCurrentAnimation();
            _currentAnimationState.Value = state;
            _currentAnimation = CreateNewAnimation(_dictionary[state]).AsObservable()
                .Subscribe(ApplyAction, WhenAnimationFinished);
        }

        private static void ApplyAction(Action action)
        {
            action.Invoke();
        }

        private void WhenAnimationFinished()
        {
            PlayAnimation(_defaultAnimation);
        }

        public void InterruptCurrentAnimation()
        {
            if (_currentAnimation != null)
                _currentAnimation.Dispose();
        }

        protected abstract AbstractAnimation<Property> CreateNewAnimation(IAnimationConfig<Property> config);

    }

}