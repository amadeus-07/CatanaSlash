using Core;
using UniRx;
using UnityEngine;

public class ShootState : State
{
    private readonly Spawner _spawner;
    private readonly Cooldown _cooldown;
    private readonly ReactiveProperty<int> _damage;
    private readonly ReactiveProperty<float> _speed;

    public ShootState(Spawner spawner, ReactiveProperty<float> duration, ReactiveProperty<int> damage, ReactiveProperty<float> speed)
    {
        _spawner = spawner;
        _cooldown = new Cooldown(duration, Attack, true);
        _damage = damage;
        _speed = speed;
    }

    protected override void OnInitialize()
    {
        _cooldown.AddTo(GlobalDisposables);
        _cooldown.Start();
    }

    protected override void OnUpdate()
    {
        _cooldown.Activate();
    }

    private void Attack()
    {
        if (Status == StateStatus.Stopped) return;
        Debug.Log("Attack");
        _spawner.Spawn((g) => InitializeProjectile(g.GetComponent<Projectile>()));
    }

    private void InitializeProjectile(Projectile projectile)
    {
        projectile.Initialize(_damage.Value, _speed.Value);
    }

}