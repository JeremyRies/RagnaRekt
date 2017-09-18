using System.Collections.Generic;

namespace Animation
{
    public interface IAnimationConfig<Property>
    {
        bool Loop { get; }
        double SecondsUntilNext { get; }
        List<Property> Steps { get; }
    }
}