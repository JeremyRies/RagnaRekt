using UniRx;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Animation {

    public abstract class AbstractAnimation<T>
    {
        private readonly IAnimationConfig<T> _config;

        protected AbstractAnimation(IAnimationConfig<T> config)
        {
            _config = config;
        }

        private static IObservable<int> CreateIndices(IAnimationConfig<T> config)
        {
            var indices = Observable.Range(0, config.Steps.Count);
            var timespan = TimeSpan.FromSeconds(config.SecondsUntilNext);
            var delayed = indices.Delay(timespan);
            return config.Loop ? delayed.Repeat() : delayed;
        }

        public IObservable<Action> AsObservable()
        {
            return CreateIndices(_config).Select(CreateAction);
        }

        private Action CreateAction(int index)
        {
            return () => Apply(_config.Steps[index]);
        }

        protected abstract void Apply(T step);

    }

}