using Core;
using UniRx;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private Cooldown _cooldown;

    [SerializeField] private Character target;

    private void Awake()
    {
        _cooldown = new Cooldown(new ReactiveProperty<float>(15), () => Spawn(InitializeEnemy) ,true);
    }
    private void Start()
    {
        _cooldown.AddTo(this);
        Spawn(InitializeEnemy);
    }

     private void Update()
    {
        _cooldown.Activate();
    }

    private void InitializeEnemy(GameObject obj)
    {
        var enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetTarget(target);
        }
    }


   
}