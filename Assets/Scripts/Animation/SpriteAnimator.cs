using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Animation {

    public class SpriteAnimator<State> : AbstractAnimator<State, Sprite> {

        private readonly SpriteRenderer _renderer;

        public SpriteAnimator(IAnimatorConfig<State, Sprite> config, SpriteRenderer renderer) 
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