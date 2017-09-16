using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class SpriteAnimatorConfig<E> : ScriptableObject, IAnimatorConfig<E, Sprite>
    {
        [SerializeField]
        private List<AnimationStep<E, Sprite>> _animationSteps;
        public List<AnimationStep<E, Sprite>> AnimationSteps { get { return _animationSteps; } }

        [SerializeField]
        private E _defaultAnimation;
        public E DefaultAnimation { get { return _defaultAnimation; } }
    }
}