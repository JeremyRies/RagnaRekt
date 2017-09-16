using UniRx;
using System;
using System.Collections.Generic;

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
            var indexCycle = config.Loop ? indices.Repeat() : indices;
            var secondsUntilNextIndex = TimeSpan.FromSeconds(config.SecondsUntilNext);
            return Observable.Interval(secondsUntilNextIndex)
                .Zip(indexCycle, (time, index) => index);
        }

        public IObservable<Action> AsObservable()
        {
            return CreateIndices(_config).Select<int, Action>(index => () => Apply(_config.Steps[index]));
        }

        protected abstract void Apply(T step);

    }

}