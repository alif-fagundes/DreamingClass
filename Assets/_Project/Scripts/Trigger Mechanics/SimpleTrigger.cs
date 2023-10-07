using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] private bool _shouldDisableAfterTrigger = false;
    [Tooltip("If a string is passed, only objects with this tag will trigger")]
    [SerializeField] private string _tagRequiredToTrigger = "Player";

    [Header("Events")]
    [SerializeField] UnityEvent _onTriggerEnterEvent;
    [SerializeField] UnityEvent _onTriggerExitEvent;

    [Header("Debug")]
    [SerializeField] bool _showGizmo;
    [SerializeField] Color _gizmoColor = Color.cyan;

    private void OnTriggerEnter(Collider collision)
    {
        if (_tagRequiredToTrigger != null && !string.IsNullOrEmpty(_tagRequiredToTrigger))
        {
            if (!collision.gameObject.CompareTag(_tagRequiredToTrigger)) return;
        }

        _onTriggerEnterEvent?.Invoke();

        if (_shouldDisableAfterTrigger)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnDrawGizmos()
    {
        if (!_showGizmo)
        {
            return;
        }

        BoxCollider collider = GetComponent<BoxCollider>();

        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (_tagRequiredToTrigger != null && !string.IsNullOrEmpty(_tagRequiredToTrigger))
        {
            if (!collision.gameObject.CompareTag(_tagRequiredToTrigger)) return;
        }

        _onTriggerExitEvent?.Invoke();

    }

}
