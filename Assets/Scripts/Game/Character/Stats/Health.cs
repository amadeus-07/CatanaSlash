using System;
using Core;
using UniRx;
using UnityEngine;

public sealed class Health : IStat
{
    public Health(int max, int current)
    {
        Max = new ReactiveProperty<int>(max);
        Current = new ReactiveProperty<int>(Mathf.Min(current, max));

        IsDead = Current
            .Select(v => v <= 0)
            .DistinctUntilChanged()
            .ToReadOnlyReactiveProperty();

        IsDead
            .Where(d => d)
            .Subscribe(_ => OnDead?.Invoke());

        Max.Subscribe(m =>
        {
            if (Current.Value > m)
                Current.Value = m;
        });
    }


    public ReactiveProperty<int> Max { get; }
    public ReactiveProperty<int> Current { get; }

    public IReadOnlyReactiveProperty<bool> IsDead { get; }
    public event Action OnDead;

    public void SetCurrent(int value)
        => Current.Value = Mathf.Clamp(value, 0, Max.Value);

    public void ChangeCurrent(int delta)
        => SetCurrent(Current.Value + delta);
}
