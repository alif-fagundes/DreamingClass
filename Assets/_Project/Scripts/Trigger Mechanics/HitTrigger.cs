using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HitTrigger : MonoBehaviour
{
    [Tooltip("Object that will be responsible to deal damage. Must implements IDoDamage interface.")]
    [SerializeField] private GameObject _damageDealer;
    [SerializeField] private GameObject[] _objectsToIgnore;
    [SerializeField] private float _timeToResetTargets = 0.3f;

    [Header("Debug")]
    [SerializeField] bool _showGizmo;
    [SerializeField] Color _gizmoColor = Color.cyan;


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
        if (_objectsToIgnore.Contains(other.gameObject)) return;

        if (other.gameObject.TryGetComponent(out CanTakeHits hittable))
        {
            if (_targetsHit.Contains(hittable)) return;

            hittable.TakeHit(_damageDealer.transform.position);
            _targetsHit.Add(hittable);

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

    private void OnDrawGizmos()
    {
        if (!_showGizmo)
        {
            return;
        }

        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider>();
        }

        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(_collider.bounds.center, _collider.bounds.size);
    }

    private IEnumerator ClearTargetsHit()
    {
        yield return new WaitForSeconds(_timeToResetTargets);
        _targetsHit.Clear();
        _collider.enabled = false;
        yield return null;
        _collider.enabled = true;
    }
}
