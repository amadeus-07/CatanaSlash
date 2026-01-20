using Core;
using UniRx;
using UnityEngine;

public class CombatState : State
{
    private readonly AttackDamage _damage;
    private readonly Cooldown _cooldown;

    private Health _health;
     

    public CombatState(Health health, AttackDamage damage, ReactiveProperty<float> duration)
    {
        _health = health;
        _damage = damage;
        _cooldown = new Cooldown(duration, Attack, true);
    }

    protected override void OnInitialize()
    {
        _cooldown.AddTo(GlobalDisposables);
        _cooldown.Start();
    }


    public void SetTarget(Health health)
        => _health = health;

    
    protected override void OnUpdate()
    {
        _cooldown.Activate();
    }

    private void Attack()
    {
        if (Status == StateStatus.Stopped) return;
        if (_health == null) return;
        if (_health.IsDead.Value) return;
        Debug.Log("Attacked");
        _health.ChangeCurrent(-_damage.Value);
    }
}
