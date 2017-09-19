using UnityEngine;
using UniRx;
using System;

namespace Animation {

    public class SpriteAnimation : AbstractAnimation<Sprite> {
        
        private readonly SpriteRenderer _renderer;

        public SpriteAnimation(IAnimationConfig<Sprite> config, SpriteRenderer renderer)
            : base(config)
        {
            _renderer = renderer;
        }

        protected override void Apply(Sprite step)
        {
            _renderer.sprite = step;
        }
    }

}