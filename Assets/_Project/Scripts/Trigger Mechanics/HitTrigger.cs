using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HitTrigger : MonoBehaviour
{
    [Tooltip("Object that will be responsible to deal damage. Must implements IDoDamage interface.")]
    [SerializeField] private GameObject _damageDealer;
    [SerializeField] private float _timeToResetTargets = 0.3f;

    private List<CanTakeHits> _targetsHit = new List<CanTakeHits>();

    private BoxCollider _collider;
    private IDoDamage _damageDealerComp;
    private Coroutine _clearTargetsCoroutine;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _damageDealerComp = _damageDealer.GetComponent<IDoDamage>();

        if (_damageDealerComp == null)
        {
            Debug.LogError($"{_damageDealer.name} does not implements IDoDamage interface");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.gameObject.name} collided with {_damageDealer.name}");

        if (other.gameObject.TryGetComponent(out CanTakeHits hittable))
        {
            if (_targetsHit.Contains(hittable)) return;

            hittable.TakeHit(_damageDealer.transform.position);

            if (other.gameObject.TryGetComponent(out ITakeDamage damageable))
            {
                _damageDealerComp.DoDamage(damageable);
            }

            if (_clearTargetsCoroutine != null)
            {
                StopCoroutine(_clearTargetsCoroutine);
            }

            _clearTargetsCoroutine = StartCoroutine(ClearTargetsHit());
        }
    }

    private IEnumerator ClearTargetsHit()
    {
        yield return new WaitForSeconds(_timeToResetTargets);
        _targetsHit.Clear();
    }
}
