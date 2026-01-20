using UnityEngine;
using UnityEngine.AI;
using Core;
using System.Linq;

public class KeepDistanceState : State
{
    private readonly Transform _owner;
    private readonly Transform _target;
    private readonly IMovementTo _movement;
    private readonly float _keepDistance;
    
    private const int SearchTries = 40;
    private const float SampleRadius = 2f;
    private const float MaxPathDistance = 50f; // Максимальная длина пути
    
    private NavMeshPath _path; // Переиспользуемый путь для проверки

    public KeepDistanceState(
        Transform owner,
        Transform target,
        IMovementTo movement,
        float keepDistance)
    {
        _owner = owner;
        _target = target;
        _movement = movement;
        _keepDistance = keepDistance;
        _path = new NavMeshPath();
    }

    protected override void OnUpdate()
    {
        if (!_target) return;
        var pos = BestPosition();
        _movement.MoveTo(pos);
    }

    private Vector3 BestPosition()
    {
        var valid = NavMesh.CalculatePath(_owner.position, _target.position, NavMesh.AllAreas, _path);
        if (!valid)
            return _owner.position;
        Vector3 candidate = _path.corners.TakeLast(2).First();
        var dir = (candidate - _target.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(_target.position, dir, out hit, _keepDistance, LayerMask.GetMask("Obstacle")))
        {
            candidate = hit.point;
        }
        else
        {
            candidate = _target.position + dir * _keepDistance;
        }
        return candidate;
    }
}