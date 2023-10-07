using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class SimpleSwitch : MonoBehaviour
{
    public bool IsOn;

    [SerializeField] string _turnOnTriggerParam = "TurnOn";
    [SerializeField] string _turnOffTriggerParam = "TurnOff";

    public UnityEvent OnTurnOn;
    public UnityEvent OnTurnOff;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ToggleSwitch(bool value)
    {
        IsOn = value;

        if (value)
        {
            _animator.SetTrigger(_turnOnTriggerParam);
            OnTurnOn?.Invoke();
        }
        else
        {
            _animator.SetTrigger(_turnOffTriggerParam);
            OnTurnOff?.Invoke();
        }
    }
    public void ToggleSwitch()
    {
        ToggleSwitch(!IsOn);
    }
}
