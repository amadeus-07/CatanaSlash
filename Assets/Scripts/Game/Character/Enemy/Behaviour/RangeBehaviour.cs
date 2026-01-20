using Core;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class RangeBehaviour : Enemy
{
    [SerializeField] private float minDistanceFollow;
    [SerializeField] private float maxDistanceFollow;
    [SerializeField] private Spawner spawner;
    private NavMeshAgent _agent;
    private StateMachine _attack_fsm;
    private StateMachine _movement_fsm;

    private float Distance {get => Vector3.Distance(transform.position, Target.transform.position); }

    private void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _attack_fsm = new StateMachine(this);
        _movement_fsm = new StateMachine(this);
    }

    private void Start()
    {
        base.Start();
        var movement = new NavMeshMovement(_agent);
        var keepDistanceState = new KeepDistanceState(transform, Target.transform, movement, minDistanceFollow);
        var shootState = new ShootState(spawner, Stats.Get<AttackSpeed>().Duration, Stats.Get<AttackDamage>(), new ReactiveProperty<float>(200));
        var idleState = new IdleState();
        _movement_fsm.Start(keepDistanceState);
        _attack_fsm.AddTransition(idleState, shootState, IsPlayerVisible);
        _attack_fsm.AddTransition(shootState, idleState, () => !IsPlayerVisible());
        _attack_fsm.Start(idleState);
    }


    private void FixedUpdate()
    {
        var rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
        Debug.Log(_attack_fsm.CurrentState);
    }


    private bool IsPlayerVisible()
    {
        Vector3 dir = Target.transform.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        if (Physics.Raycast(transform.position, dir, out var hit, 100f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green); // Видим, куда попал луч
            
            if (hit.collider.gameObject == Target.gameObject)
            {
                return true;
            }
        }
        
        Debug.DrawRay(transform.position, dir*100f, Color.red);
        return false;
    }

}
