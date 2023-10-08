using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class PlayerController : BaseController, ICanActivatePressurePlates, IDoDamage, ITakeDamage
{
    public static PlayerController Instance;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isTankMovement;

    [Header("Combat")]
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private float _attackCooldown = 1.2f;
    [SerializeField] private int _attackDamage = 10;
    public bool IsDead = false;

    [Header("Events")]
    public UnityEvent OnAttack;
    public UnityEvent OnDeath;

    private InputManager _input;
    private CharacterController _characterController;
    private Animator _animator;

    private bool _canAttack = true;
    private Coroutine _attackCooldownCoroutine;

    private int _currentHealth;
    private Camera _mainCam;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _input = GetComponent<InputManager>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _mainCam = Camera.main;

        ToggleEnabledCallback = ToggleInputs;

        _currentHealth = _maxHealth;
    }

    public void Update()
    {
        if (!IsEnabled) return;
        if (IsDead) return;


        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float horizontalInput = _input.Move.x;
        float verticalInput = _input.Move.y;

        Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput);
        if (inputDirection == Vector3.zero) return;

        if (_isTankMovement)
        {
            // Rotate around y - axis
            transform.Rotate(0, horizontalInput * _rotateSpeed, 0);

            // Move forward / backward
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            float curSpeed = _moveSpeed * verticalInput;
            _characterController.SimpleMove(forward * curSpeed);
        }
        else
        {
            // Get the camera's forward and right vectors.
            Vector3 cameraForward = _mainCam.transform.forward;
            Vector3 cameraRight = _mainCam.transform.right;

            // Ignore the y-axis to ensure movement stays on the ground.
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Normalize vectors to prevent faster diagonal movement.
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Get the input direction based on camera orientation.
            inputDirection = cameraForward * verticalInput + cameraRight * horizontalInput;

            transform.forward = (transform.position + inputDirection) - transform.position;
            _characterController.SimpleMove(inputDirection * _moveSpeed);
        }
    }

    private void HandleAttack()
    {
        if (_input.Attack)
        {
            _input.Attack = false;

            if(!_canAttack) return;

            _canAttack = false;
            _animator.SetTrigger("Attack");
            OnAttack?.Invoke();

            if(_attackCooldownCoroutine != null)
            {
                StopCoroutine(_attackCooldownCoroutine);
            }

            _attackCooldownCoroutine = StartCoroutine(EnableAttackAfterCooldown());
        }
    }

    private IEnumerator EnableAttackAfterCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    public void DoDamage(ITakeDamage other)
    {
        if (IsDead) return;
        other.TakeDamage(_attackDamage);
    }

    private void ToggleInputs(bool value)
    {
        _input.ToggleEnabled(value);   
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        if (IsDead) return;
        ToggleEnabled(false);
        IsDead = true;
        OnDeath?.Invoke();
    }
}
