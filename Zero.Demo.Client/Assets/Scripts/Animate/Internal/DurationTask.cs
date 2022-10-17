using System;
using UnityEngine;

namespace AnimateInternal
{
    public class DurationTask : AnimateTask
    {
        private float _duration;
        private Action<float> _stepAction;
        private float _time;
        private EasingType _easingType;

        public DurationTask Duration(float duration)
        {
            _duration = duration;
            return this;
        }

        public DurationTask Ease(EasingType easingType)
        {
            _easingType = easingType;
            return this;
        }

        public void Setup(float duration, Action<float> stepAction)
        {
            SetupTask();

            _easingType = EasingType.None;
            _duration = duration;
            _stepAction = stepAction;
        }

        protected override bool DoUpdate()
        {
            _time += Time.deltaTime;
            var step = Mathf.Clamp01(_time / _duration);
            _stepAction?.Invoke(Easing.Function(_easingType, step));
            return _time >= _duration;
        }
    }
}
