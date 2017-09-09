using System;
using System.Timers;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class Cooldown
    {
        private readonly float _cooldownTime;

        private Timer _timer;
        private bool _onCooldown;

        public bool IsOnCoolDown { get { return _onCooldown; } }

        public event Action IsOffCooldown;

        public Cooldown(float cooldownTimeInSeconds)
        {
            _cooldownTime = cooldownTimeInSeconds;
            ResetTimer();
        }

        private void ResetTimer()
        {
            _timer = new Timer(_cooldownTime * 1000);
            _timer.Elapsed += WhenTimerElapsed;
            _timer.BeginInit();
        }

        public void Start()
        {
            _onCooldown = true;
            _timer.Start();
        }

        private void WhenTimerElapsed(object nil, ElapsedEventArgs args)
        {
            _onCooldown = false;
            if (IsOffCooldown != null) IsOffCooldown();
            ResetTimer();
        }
    }
}