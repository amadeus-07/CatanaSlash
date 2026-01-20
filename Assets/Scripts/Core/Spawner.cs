using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform point;


    public void Spawn(Action<GameObject> initailize)
    {
        var instance = Instantiate(prefab, point.position, point.rotation);
        initailize(instance);
    }

    
}