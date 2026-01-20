using System;
using System.Linq;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : Character
{
    private InputSystem_Actions _actions;
    private NavMeshAgent _agent;
    private IMovement _movement;
    private Cooldown _cooldown;

    private void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _movement = new NavMeshMovement(_agent, true);
        _actions = new InputSystem_Actions();
        _actions.Enable();
    }

    private void Start()
    {
        base.Start();
        _cooldown = new Cooldown(Stats.Get<AttackSpeed>().Duration, Attack, true).AddTo(this); 
        _actions.Player.Attack.performed += OnAttackPerformed;
    }

    

    private void Update()
    {
        var speed = Stats.Get<Speed>();
        var input = _actions.Player.Move.ReadValue<Vector2>().normalized;
        
        _movement.Move(new Vector3(input.x, 0, input.y) * speed.Value * Time.deltaTime);
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        _cooldown.Activate();
    }

    private void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (var col in hitColliders)
        {
            var stats = col.GetComponent<StatsContext>();
            if (stats != null && stats != Stats)
            {
                var health = stats.Get<Health>();
                Debug.Log($"{stats.gameObject.name}: {health.Current}");
                health.ChangeCurrent(-Stats.Get<AttackDamage>().Value);
            }
        }
    }


    private void OnDestroy()
    {
        base.OnDestroy();
        _actions?.Disable();
    }
}