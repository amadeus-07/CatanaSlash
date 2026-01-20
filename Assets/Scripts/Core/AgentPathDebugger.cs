using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer), typeof(NavMeshAgent))]
public class AgentPathDebugger : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private NavMeshAgent _agent;

    
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_agent.hasPath)
        {
            _lineRenderer.positionCount = _agent.path.corners.Length;
            _lineRenderer.SetPositions(_agent.path.corners);
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}