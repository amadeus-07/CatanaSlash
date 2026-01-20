using System;
using UniRx;
using UnityEngine;

namespace Core
{
    public enum CooldownState
    {
        Ready,
        Running,
        Paused
    }
    
    public sealed class Cooldown
    {
        private readonly Timer _timer;
        private IDisposable _tickSubscription;
        private readonly Action _action; // Действие, которое выполняется при активации

        public ReactiveProperty<float> Duration => _timer.Duration;
        public ReactiveProperty<CooldownState> State { get; }

        // Добавляем Action в конструктор
        public Cooldown(ReactiveProperty<float> duration, Action action, bool startActive = false)
        {
            _action = action;
            _timer = new Timer(duration, startActive);
            State = new ReactiveProperty<CooldownState>(startActive ? CooldownState.Running : CooldownState.Ready);

            _timer.State.Subscribe(timerState =>
            {
                switch (timerState)
                {
                    case TimerState.Ready:
                        State.Value = CooldownState.Ready;
                        break;
                    case TimerState.Running:
                        State.Value = CooldownState.Running;
                        break;
                    case TimerState.Paused:
                        State.Value = CooldownState.Paused;
                        break;
                }
            });
        }

        /// <summary>
        /// Главный метод: если готов — стреляем и уходим в КД. 
        /// Если не готов — ничего не делаем.
        /// </summary>
        public void Activate()
        {
            if (State.Value == CooldownState.Ready)
            {
                _action?.Invoke(); // Выполняем само действие
                Start();           // Запускаем отсчет кулдауна
            }
        }

        public bool Start()
        {
            if (State.Value != CooldownState.Ready) return false;

            _timer.Start(Duration.Value);
            // State обновится автоматически через Subscribe на _timer.State
            return true;
        }

        public void Pause() => _timer.Pause();
        public void Resume() => _timer.Resume();
        
        public void Reset()
        {
            _timer.Reset();
            State.Value = CooldownState.Ready;
        }

        public void Rollback(float seconds)
        {
            if (State.Value == CooldownState.Ready) return;

            _timer.Remaining.Value = Mathf.Max(0f, _timer.Remaining.Value - seconds);
            // Если таймер упал до 0, он сам перейдет в Ready через уведомление от Timer
        }

        public Cooldown AddTo(CompositeDisposable disposable)
        {
            _tickSubscription?.Dispose();
            _tickSubscription = _timer.AddTo(disposable);
            return this;
        }
        public Cooldown AddTo(Component component)
        {
            _tickSubscription?.Dispose();
            // Используем стандартный метод UniRx для привязки к GameObject
            _tickSubscription = _timer.AddTo(component);
            return this;
        }

        public void Remove()
        {
            _tickSubscription?.Dispose();
            _tickSubscription = null;
        }
    }
}

