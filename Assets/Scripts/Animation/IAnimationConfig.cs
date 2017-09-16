using System.Collections.Generic;

namespace Animation
{
    public interface IAnimationConfig<A>
    {
        bool Loop { get; }
        double SecondsUntilNext { get; }
        List<A> Steps { get; }
    }
}