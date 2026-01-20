using System;
using System.Collections.Generic;
using System.Linq; // Добавлено для фильтрации
using UniRx;
using UnityEngine;

namespace Core
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }
        
        // Используем структуру или кортеж, включающий UpdateType
        private readonly List<Transition> _transitions = new();
        private CompositeDisposable _disposables;

        // Вспомогательная структура для хранения данных о переходе
        private struct Transition
        {
            public State From;
            public State To;
            public Func<bool> Condition;
            public UpdateType UpdateType;
        }

        public StateMachine(Component component)
        {
            _disposables = new CompositeDisposable();

            Observable.EveryUpdate()
                .Subscribe(_ => Tick())
                .AddTo(_disposables);

            Observable.EveryFixedUpdate()
                .Subscribe(_ => FixedTick())
                .AddTo(_disposables);

            _disposables.AddTo(component);
        }

        public void AddTransition(State from, State to, Func<bool> condition, UpdateType updateType = UpdateType.Render)
        {
            // Инициализируем стейты, если это необходимо
            from.Initialize(_disposables);
            to.Initialize(_disposables);

            _transitions.Add(new Transition 
            { 
                From = from, 
                To = to, 
                Condition = condition, 
                UpdateType = updateType 
            });
        }

        public void SetState(State state)
        {
            if (CurrentState == state) return;

            CurrentState?.Exit();
            Start(state);
        }

        public void Start(State initialState)
        {
            CurrentState = initialState;
            CurrentState.Enter();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }

        private void Tick()
        {
            // Проверяем переходы, помеченные как Render (Update)
            TryTransition(UpdateType.Render);
            CurrentState?.Update();
        }

        private void FixedTick()
        {
            // Проверяем переходы, помеченные как Physics (FixedUpdate)
            TryTransition(UpdateType.Physics);
            CurrentState?.FixedUpdate();
        }

        private void TryTransition(UpdateType type)
        {
            if (CurrentState == null) return;

            // Проходим только по тем транзициям, которые соответствуют текущему циклу обновления
            foreach (var transition in _transitions)
            {
                if (transition.UpdateType == type && transition.From == CurrentState)
                {
                    if (transition.Condition())
                    {
                        SetState(transition.To);
                        break; 
                    }
                }
            }
        }
    }

    public enum UpdateType
    {
        Render,
        Physics
    }
}