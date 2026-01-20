using Core;
using UniRx;

public sealed class AttackDamage : ReactiveProperty<int>, IStat
{
    public AttackDamage(int value)
    {
        Value = value;
    }
}