using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation {

    [Serializable]
    public class SpriteAnimationConfig : IAnimationConfig<Sprite>
    {

        [SerializeField]
        private double _secondsUntilNext;
        public bool Loop { get { return _loop; } }

        [SerializeField]
        private bool _loop;
        public double SecondsUntilNext { get { return _secondsUntilNext; } }

        [SerializeField]
        private List<Sprite> _sprites;
        public List<Sprite> Steps { get { return _sprites; } }

    }

}