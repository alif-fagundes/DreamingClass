using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    [Header("Character Input Values")]
    [SerializeField] PlayerInput _playerInput;
    public Vector2 Move;
    public bool Attack = false;
    public bool Pause = false;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }
    public void OnPause(InputValue value)
    {
        PauseInput(value.isPressed);
    }

    public void MoveInput(Vector2 newState)
    {
        Move = newState;
    }

    public void PauseInput(bool newState)
    {
        Pause = newState;
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
