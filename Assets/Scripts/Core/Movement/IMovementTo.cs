using UnityEngine;

namespace Core
{
    public interface IMovementTo
    {
        public void MoveTo(Vector3 input);
        public void Stop();
    }
}