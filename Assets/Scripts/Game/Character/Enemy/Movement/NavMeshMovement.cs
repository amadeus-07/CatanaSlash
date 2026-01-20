using Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshMovement : IMovementTo, IMovement
{
    private readonly NavMeshAgent _agent;
    private readonly bool _isControlled;

    public NavMeshMovement(NavMeshAgent agent, bool isControlled = false)
    {
        _agent = agent;
        _isControlled = isControlled;
    }

    public void Move(Vector3 input)
    {
        if (_isControlled)
            _agent.Move(input);
        else
            _agent.destination = _agent.transform.position + input;
        _agent.isStopped = false;
        
    }

    public void MoveTo(Vector3 target)
    {
        _agent.destination = target;
        _agent.isStopped = false;
    }

    public void Stop()
    {
        _agent.isStopped = true;
    }
}
