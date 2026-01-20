using Core;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBehaviour : Enemy, IBehaviour
{
    [SerializeField] private float minDistanceFollow;
    [SerializeField] private float maxDistanceFollow;
    private NavMeshAgent _agent;
    private StateMachine _fsm;

    private float Distance {get => Vector3.Distance(transform.position, Target.transform.position); }

    private void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = minDistanceFollow;
        _fsm = new StateMachine(this);
    }

    private void Start()
    {
        base.Start();
        var idle = new IdleState();
        var follow = new FollowState(Target.transform, new NavMeshMovement(_agent));
        var combat = new CombatState(Target.Stats.Get<Health>(), Stats.Get<AttackDamage>(), Stats.Get<AttackSpeed>().Duration);
        _fsm.AddTransition(follow, idle, () => Distance <= minDistanceFollow);
        _fsm.AddTransition(idle, follow, () => Distance >= maxDistanceFollow);
        _fsm.AddTransition(idle, combat, CanAttack);
        _fsm.AddTransition(combat, idle, () => !CanAttack());
        _fsm.Start(idle);
    }


    private bool CanAttack()
    {
        if ( Distance > maxDistanceFollow) return false;
        var isDead = Target.Stats.Get<Health>().IsDead.Value;
        return !isDead;
    }

}