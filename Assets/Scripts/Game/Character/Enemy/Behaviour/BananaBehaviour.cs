using Core;
using UniRx;
using UnityEngine;


public class BananaBehaviour : Enemy
{
    [SerializeField] private float minDistanceToAttack;

    private StateMachine _fsm;

    private float Distance {get => Vector3.Distance(transform.position, Target.transform.position); }

    private void Awake()
    {
        base.Awake();
        _fsm = new StateMachine(this);
    }

     private void Start()
    {
        base.Start();
        var idle = new IdleState();
        var follow = new FollowState(Target.transform, new TransformMovement(transform, new ReactiveProperty<float>(3.5f), Axis.Y));
        var combat = new CombatState(Target.Stats.Get<Health>(), Stats.Get<AttackDamage>(), Stats.Get<AttackSpeed>().Duration);
         _fsm.AddTransition(follow, idle, () => Distance < minDistanceToAttack);
        _fsm.AddTransition(idle, follow, () => Distance >= minDistanceToAttack && !CanAttack());
        _fsm.AddTransition(idle, combat, CanAttack);
        _fsm.AddTransition(combat, idle, () => Distance >= minDistanceToAttack);
        _fsm.Start(idle);
    }

    private bool CanAttack()
    {
        if ( Distance > minDistanceToAttack) return false;
        var isDead = Target.Stats.Get<Health>().IsDead.Value;
        return !isDead;
    }
}