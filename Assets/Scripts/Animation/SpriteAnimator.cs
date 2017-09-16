using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Animation {

    public class SpriteAnimator<E> : AbstractAnimator<E, Sprite> {

        private readonly SpriteRenderer _renderer;

        public SpriteAnimator(IAnimatorConfig<E, Sprite> config, SpriteRenderer renderer) 
            : base(config)
        {
            _renderer = renderer;
        }

        protected override AbstractAnimation<Sprite> CreateNewAnimation(IAnimationConfig<Sprite> config)
        {
            return new SpriteAnimation(config, _renderer);
        }
    }

}