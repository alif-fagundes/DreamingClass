using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform _turretHead;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private bool _shouldAimAtPlayer = false;
    [SerializeField] private float _detectionArea = 10f;
    [SerializeField] private float _timeBeforeReseting = 3f;
    [SerializeField] private string _damageOnlyTag;
    [SerializeField] private LayerMask _wallLayer;
    public bool isEnabled = true;

    [Header("Combat")]
    [SerializeField] private float _shootCooldown = 0.4f;
    [SerializeField] private int _shootDamage = 2;
    [SerializeField] private TurretProjectile _projectilePrefab;
    [SerializeField] private bool _canTakeDamage = false;
    [Tooltip("Only used if canTakeDamage is turned on")]
    [SerializeField] private int _MaxHealth = 10;

    [Header("Events")]
    public UnityEvent OnShoot;

    [Header("Debug")]
    [SerializeField] private bool _shouldDebug;
    [SerializeField] private Color _detectionAreaColor = new Color(1, 0.8f, 0.5f, 0.4f);

    private float _lastShootTime = 0f;

    private Vector3 _startHeadRotation;
    private bool _playerInSight = false;
    private float _lastTimePlayerWasInSight = 0f;
    private float _detectionCd = 1f;
    private float _lastDetectionTime = 0f;
    private Coroutine _resetCoroutine;

    private void Awake()
    {
        _startHeadRotation = _turretHead.transform.forward;
    }

    private void Update()
    {
        if (!isEnabled) return;

        if (_shouldAimAtPlayer)
        {
            PlayerDetection();
        }

        if (Time.time - _lastShootTime > _shootCooldown)
        {
            if (_shouldAimAtPlayer && !_playerInSight) return;

            Shoot();
        }
    }

    private void PlayerDetection()
    {
        Vector3 targetLocation = PlayerController.Instance.transform.position;

        float distance = Vector3.Distance(transform.position, targetLocation);

        if (Time.time - _lastDetectionTime > _detectionCd)
        {
            // should try detecting player

            _lastDetectionTime = Time.time;
            _playerInSight = false;

            if (distance <= _detectionArea)
            {
                // check if there are no walls between turret and target

                if (!Physics.Linecast(new Vector3(_shootingPoint.position.x, _shootingPoint.position.y + 2f, _shootingPoint.position.z),
                    new Vector3(targetLocation.x, targetLocation.y + 2f, targetLocation.z), out RaycastHit hit, _wallLayer))
                {

                    _playerInSight = true;
                    _lastTimePlayerWasInSight = Time.time;
                }
            }
        }


        if (_playerInSight)
        {
            if (_resetCoroutine != null)
            {
                StopCoroutine(_resetCoroutine);
            }

            // Determine which direction to rotate towards
            Vector3 targetDirection = targetLocation - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = _rotationSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(_turretHead.forward, targetDirection, singleStep, 0.0f);

            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            _turretHead.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            if (_startHeadRotation == _turretHead.transform.forward) return;

            if (Time.time - _lastTimePlayerWasInSight > _timeBeforeReseting)
            {
                if (_resetCoroutine == null)
                {
                    _resetCoroutine = StartCoroutine(ResetRotation());
                }

            }
        }
    }

    private void Shoot()
    {
        _lastShootTime = Time.time;

        var _newProjectile = Instantiate(_projectilePrefab, _shootingPoint.position, _turretHead.transform.rotation);
        _newProjectile.DamageOnlyTag = _damageOnlyTag;
        _newProjectile.Damage = _shootDamage;
        _newProjectile.Owner = this;

        OnShoot?.Invoke();
    }

    private IEnumerator ResetRotation()
    {
        while (_turretHead.forward != _startHeadRotation)
        {
            // The step size is equal to speed times frame time.
            var speed = _rotationSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(_turretHead.forward, _startHeadRotation, speed, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            _turretHead.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }

        _resetCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        if (!_shouldDebug) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_shootingPoint.position, _shootingPoint.position + (_turretHead.forward * 20f));

        if (_shouldAimAtPlayer)
        {
            Gizmos.color = _detectionAreaColor;
            Gizmos.DrawSphere(transform.position, _detectionArea);
        }
    }
}
