using Core;
using UnityEngine;

public class TrnasformMovement : MonoBehaviour, IMovementTo
{
    private Transform _transform;

    public TrnasformMovement(Transform transform)
    {
        _transform = transform;

    }
    public void MoveTo(Vector3 position)
    {
        _transform.position = Vector3.MoveTowards(_transform.position, position, 3.5f * Time.deltaTime);
    }

    public void Stop()
    {
        
    }
}