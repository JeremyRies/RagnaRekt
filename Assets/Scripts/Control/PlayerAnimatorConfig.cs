using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using UnityEngine;

namespace Control
{
    [Serializable]
    public struct X
    {
        public PlayerAnimationState Name;
        public SpriteAnimationConfig Animation;
    }

    [CreateAssetMenu(fileName = "PlayerAnimatorConfig", menuName = "Game/PlayerAnimatorConfig")]
    public class PlayerAnimatorConfig : ScriptableObject, IAnimatorConfig<PlayerAnimationState, Sprite>
    {
        [SerializeField]
        private List<X> _animationSteps;
        public List<AnimationStep<PlayerAnimationState, Sprite>> AnimationSteps
        {
            get
            {
                return _animationSteps.Select(step => new AnimationStep<PlayerAnimationState, Sprite>
                {
                    Name = step.Name,
                    Animation = step.Animation
                }).ToList();
            }
        }

        [SerializeField]
        private PlayerAnimationState _defaultAnimation;
        public PlayerAnimationState DefaultAnimation { get { return _defaultAnimation; } }
    }
}