using UnityEngine;
using UniRx;
using Core; // Предполагаю, что используется UniRx для ReactiveProperty

public enum Axis { None, X, Y, Z }

public sealed class TransformMovement : IMovementTo
{
    

    private readonly Transform _transform;
    private readonly ReactiveProperty<float> _speed;
    private readonly Axis _lockAxis;

    public TransformMovement(Transform transform, ReactiveProperty<float> speed, Axis lockAxis = Axis.None)
    {
        _transform = transform;
        _speed = speed;
        _lockAxis = lockAxis;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        Vector3 currentPosition = _transform.position;
        Vector3 destination = targetPosition;

        // Применяем блокировку выбранной оси
        switch (_lockAxis)
        {
            case Axis.X: destination.x = currentPosition.x; break;
            case Axis.Y: destination.y = currentPosition.y; break;
            case Axis.Z: destination.z = currentPosition.z; break;
        }

        _transform.position = Vector3.MoveTowards(
            currentPosition, 
            destination, 
            Time.deltaTime * _speed.Value
        );
    }

    public void Stop()
    {
        // Логика остановки, если требуется
    }
}