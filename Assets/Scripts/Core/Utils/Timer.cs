using System;
using UniRx;
using UnityEngine;

namespace Core
{
    public enum TimerState
    {
        Ready,
        Running,
        Paused
    }

    public sealed class Timer
    {
        private float _time;
        private float _initialDuration;

        public ReactiveProperty<float> Remaining { get; }
        public ReactiveProperty<float> Duration { get; }
        public ReactiveProperty<TimerState> State { get; }

        public Timer(ReactiveProperty<float> duration, bool startActive = false)
        {
            Duration = duration;
            _initialDuration = duration.Value;
            _time = startActive ? _initialDuration : 0f;
            Remaining = new ReactiveProperty<float>(startActive ? _time : 0f);
            State = new ReactiveProperty<TimerState>(startActive ? TimerState.Running : TimerState.Ready);

            Duration.Skip(1).Subscribe(newValue =>
            {
                _initialDuration = newValue;
                if (State.Value != TimerState.Running) return;
                _time = Mathf.Min(_time, newValue);
                Remaining.Value = Mathf.Min(Remaining.Value, newValue);
            });
        }

        public void Start(float? duration = null)
        {
            _initialDuration = duration ?? _initialDuration;
            _time = _initialDuration;
            Remaining.Value = _time;
            State.Value = TimerState.Running;
        }

        public void Pause()
        {
            if (State.Value == TimerState.Running)
                State.Value = TimerState.Paused;
        }

        public void Resume()
        {
            if (State.Value == TimerState.Paused)
                State.Value = TimerState.Running;
        }

        public void Reset()
        {
            _time = _initialDuration;
            Remaining.Value = _time;
            State.Value = TimerState.Ready;
        }

        public void Stop() => State.Value = TimerState.Ready;

        public void Tick(float deltaTime)
        {
            if (State.Value != TimerState.Running) return;

            _time -= deltaTime;
            Remaining.Value = Mathf.Max(0f, _time);

            if (_time <= 0f)
                State.Value = TimerState.Ready;
        }

        public IDisposable AddTo(CompositeDisposable disposable)
        {
            return Observable.EveryUpdate()
                .Subscribe(_ => Tick(Time.deltaTime))
                .AddTo(disposable);
        }

        public IDisposable AddTo(Component component)
        {
            return Observable.EveryUpdate()
                .Subscribe(_ => Tick(Time.deltaTime))
                .AddTo(component); // Использует встроенное расширение UniRx
        }
    }
}
