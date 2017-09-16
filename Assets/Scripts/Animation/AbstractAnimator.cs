using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Animation {

    public abstract class AbstractAnimator<E, T>
    {
        private readonly IDictionary<E, IAnimationConfig<T>> _dictionary;
        private readonly E _defaultAnimation;

        private IDisposable _currentAnimation;

        protected AbstractAnimator(IAnimatorConfig<E, T> config)
        {
            _dictionary = CreateDictionary(config.AnimationSteps);
            _defaultAnimation = config.DefaultAnimation;
            _nextAnimation = _defaultAnimation;

            _currentAnimationName = new ReactiveProperty<E>(_defaultAnimation);
        }

        private static IDictionary<E, IAnimationConfig<T>> CreateDictionary(List<AnimationStep<E, T>> steps)
        {
            var result = new Dictionary<E, IAnimationConfig<T>>();
            steps.ForEach(step => result.Add(step.Name, step.Animation));
            return result;
        }
        
        private E _nextAnimation;
        public E NextAnimation { set { _nextAnimation = value; } }

        private readonly ReactiveProperty<E> _currentAnimationName;
        public IObservable<E> CurrentAnimation { get { return _currentAnimationName; } }

        public void PlayAnimation(E name)
        {
            InterruptCurrentAnimation();
            var animation = CreateNewAnimation(_dictionary[name]);
            _currentAnimationName.Value = name;
            _currentAnimation = animation.AsObservable().Subscribe(ApplyAction, WhenAnimationFinished);
        }

        private void ApplyAction(Action action)
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

        protected abstract AbstractAnimation<T> CreateNewAnimation(IAnimationConfig<T> config);

    }

}