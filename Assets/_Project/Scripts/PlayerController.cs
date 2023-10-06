using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] bool _isTankMovement;

    [Header("Interaction")]
    [SerializeField] float _interactRange;

    InputManager _input;
    CharacterController _characterController;
    
    public void Awake()
    {
        _input = GetComponent<InputManager>();
        _characterController = GetComponent<CharacterController>();
    }

    public void Update()
    {
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

        if(_isTankMovement)
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
        if(_input.Interact)
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
            Debug.Log("Player Attacked");
            _input.Attack = false;
        }
    }


}
