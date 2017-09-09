using System;
using UniRx;

namespace Assets.Scripts.Util
{
    public class Cooldown
    {
        private readonly float _cooldownTime;

        private readonly ReactiveProperty<bool> _onCooldown = new ReactiveProperty<bool>(false);
        private IDisposable _disposable;

        public ReactiveProperty<bool> IsOnCoolDown { get { return _onCooldown.ToReactiveProperty(); } }

        public Cooldown(float cooldownTimeInSeconds)
        {
            _cooldownTime = cooldownTimeInSeconds;
        }

        public void Start()
        {
            if (_onCooldown.Value) return;
            if(_disposable != null)
            {
                _disposable.Dispose();
            }

            _onCooldown.Value = true;
            _disposable = Observable.Timer(TimeSpan.FromSeconds(_cooldownTime))
                .Subscribe(_ => _onCooldown.Value = false);
        }
    }
}