using UniRx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Animation {

    public abstract class AbstractAnimation<Property>
    {
        private readonly IAnimationConfig<Property> _config;

        protected AbstractAnimation(IAnimationConfig<Property> config)
        {
            _config = config;
        }

        private static IObservable<int> CreateIndices(IAnimationConfig<Property> config)
        {
            var timespan = Observable.Interval(TimeSpan.FromSeconds(config.SecondsUntilNext)).StartWith(-1);
            var indices = timespan.Select(index => (int) (index + 1) % config.Steps.Count);
            return config.Loop ? indices : indices.Take(config.Steps.Count + 1);
        }

        public IObservable<Action> AsObservable()
        {
            return CreateIndices(_config).Select(CreateAction);
        }

        private Action CreateAction(int index)
        {
            return () => Apply(_config.Steps[index]);
        }

        protected abstract void Apply(Property step);

    }

}