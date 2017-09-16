﻿using System.Collections.Generic;

namespace Animation
{
    public struct AnimationStep<E, T>
    {
        public E Name;
        public IAnimationConfig<T> Animation;
    }

    public interface IAnimatorConfig<E, T>
    {
        List<AnimationStep<E, T>> AnimationSteps { get; }
        E DefaultAnimation { get; }
    }
}