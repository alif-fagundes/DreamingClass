using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorBehavior : MonoBehaviour
{
    [SerializeField] private string openTriggerParam = "Open";
    [SerializeField] private string closeTriggerParam = "Close";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        _animator.SetTrigger(openTriggerParam);
    }

    public void Close()
    {
        _animator.SetTrigger(closeTriggerParam);
    }
}
