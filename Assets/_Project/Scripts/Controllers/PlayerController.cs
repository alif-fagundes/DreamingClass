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

    public void Awake()
    {
        _input = GetComponent<InputManager>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        ToggleEnabledCallback = ToggleInputs;

        _currentHealth = _maxHealth;

        ToggleEnabled(true);

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

        Debug.Log("GAME OVER!!!!!!!!");
    }
}
