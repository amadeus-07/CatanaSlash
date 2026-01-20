using UnityEngine;

namespace Core
{
    public interface IMovement
    {
        public void Move(Vector3 input);
        public void Stop();
    }
} 

