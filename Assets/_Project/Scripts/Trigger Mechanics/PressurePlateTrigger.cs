using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
public class PressurePlateTrigger : MonoBehaviour
{
    [SerializeField] bool _shouldDisableAfterPressed = false;
    public bool IsPressed = false;

    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    private Animator _animator;
    private BoxCollider _collider;
    private List<ICanActivatePressurePlates> _objectsPressing = new List<ICanActivatePressurePlates>();



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name}");
        if(other.gameObject.TryGetComponent(out ICanActivatePressurePlates obj))
        {
            Debug.Log($"has interface");

            if (!_objectsPressing.Contains(obj))
            {
                _objectsPressing.Add(obj);

                if(!IsPressed)
                {
                    TogglePressurePlate(true);

                    if (_shouldDisableAfterPressed)
                    {
                        _collider.enabled = false;
                        this.enabled = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ICanActivatePressurePlates obj))
        {
            if (_objectsPressing.Contains(obj))
            {
                _objectsPressing.Remove(obj);

                if(_objectsPressing.Count == 0)
                {
                    TogglePressurePlate(false);
                }
            }
        }
    }

    private void TogglePressurePlate(bool value)
    {
        if (value)
        {
            _animator.SetTrigger("Pressed");
            OnPressed?.Invoke();
        }
        else
        {
            _animator.SetTrigger("Released");
            OnReleased?.Invoke();
        }

        IsPressed = value;
    }


}
