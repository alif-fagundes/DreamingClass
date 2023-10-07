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
public class PlayerController : BaseController, ICanActivatePressurePlates, IDoDamage
{

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isTankMovement;

    [Header("Attack")]
    [SerializeField] private float _attackCooldown = 1.2f;
    [SerializeField] private int _attackDamage = 10;

    [Header("Interaction")]
    [SerializeField] private float _interactRange;

    [Header("Events")]
    public UnityEvent OnAttack;

    private InputManager _input;
    private CharacterController _characterController;
    private Animator _animator;

    private bool _canAttack = true;
    private Coroutine _attackCooldownCoroutine;

    public void Awake()
    {
        _input = GetComponent<InputManager>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        ToggleEnabledCallback = ToggleInputs;

        ToggleEnabled(true);
    }

    public void Update()
    {
        if (!IsEnabled) return;

        HandleMovement();
        HandleInteraction();
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

    private void HandleInteraction()
    {
        if (_input.Interact)
        {
            Debug.Log("Player interacted");
            _input.Interact = false;

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
            //{
            //    Interactable interactable = hit.collider.GetComponent<Interactable>();
            //    if (interactable != null)
            //    {
            //        interactable.Interact();
            //    }
            //}
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
        other.TakeDamage(_attackDamage);
    }

    private void ToggleInputs(bool value)
    {
        _input.ToggleEnabled(value);   
    }
}
