using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class TurretProjectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _speed = 5f;

    [Header("Set by the turret:")]
    public int Damage;
    public Turret Owner;
    public string DamageOnlyTag;

    private Collider _collider;

    private Vector3 _startingPosition;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _startingPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _startingPosition + (transform.forward * 20f), Time.deltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(DamageOnlyTag))
        {
            if (!other.CompareTag(DamageOnlyTag))
            {
                DestroyProjectile();
                return;
            }
        }

        if (other.gameObject.TryGetComponent(out CanTakeHits hittable))
        {
            hittable.TakeHit(transform.position);
        }

        if (other.gameObject.TryGetComponent(out ITakeDamage damageable))
        {
            damageable.TakeDamage(Damage);
        }

        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (_particleSystem != null)
        {
            Instantiate(_particleSystem.gameObject, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
