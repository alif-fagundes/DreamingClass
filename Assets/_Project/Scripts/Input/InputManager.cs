using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    [Header("Character Input Values")]
    [SerializeField] PlayerInput _playerInput;
    public Vector2 Move;
    public bool Interact = false;
    public bool Attack = false;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnInteract(InputValue value)
    {
        InteractInput(value.isPressed);
    }

    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }

    public void MoveInput(Vector2 newState)
    {
        Move = newState;
    }

    public void InteractInput(bool newState)
    {
        Interact = newState;
    }

    public void AttackInput(bool newState)
    {
        Attack = newState;
    }

    public void ToggleEnabled(bool value)
    {
        _playerInput.enabled = value;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
