using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private int _damage;
    private float _speed;
    private Rigidbody _rb;

    public void Initialize(int damage, float speed)
    {
        _damage = damage;
        _speed = speed;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rb.AddForce(transform.forward * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.Stats.Get<Health>().ChangeCurrent(-_damage);
        }
        Destroy(gameObject);
    }
}