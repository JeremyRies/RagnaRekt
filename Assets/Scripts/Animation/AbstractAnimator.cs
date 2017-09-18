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

        protected AbstractAnimator(IAnimatorConfig<State, Property> config)
        {
            _dictionary = CreateDictionary(config.AnimationSteps);
            _defaultAnimation = config.DefaultAnimation;
            _nextAnimation = _defaultAnimation;

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
        
        private State _nextAnimation;
        public State NextAnimation { set { _nextAnimation = value; } }

        private readonly ReactiveProperty<State> _currentAnimationState;
        public IObservable<State> ShownAnimation { get { return _currentAnimationState; } }
        public State CurrentAnimation { get { return _currentAnimationState.Value; } }

        public void PlayAnimation(State name)
        {
            InterruptCurrentAnimation();
            _currentAnimationState.Value = name;
            _currentAnimation = CreateNewAnimation(_dictionary[name]).AsObservable()
                .Subscribe(ApplyAction, WhenAnimationFinished);
        }

        private static void ApplyAction(Action action)
        {
            action.Invoke();
        }

        private void WhenAnimationFinished()
        {
            PlayAnimation(_nextAnimation);
            _nextAnimation = _defaultAnimation;
        }

        public void InterruptCurrentAnimation()
        {
            if (_currentAnimation != null)
                _currentAnimation.Dispose();
        }

        protected abstract AbstractAnimation<Property> CreateNewAnimation(IAnimationConfig<Property> config);

    }

}