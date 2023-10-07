using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] bool _shouldDisableAfterTrigger = false;
    [SerializeField] UnityEvent _onTriggerEnterEvent;

    [Header("Debug")]
    [SerializeField] bool _showGizmo;
    [SerializeField] Color _gizmoColor = Color.cyan;

    private void OnTriggerEnter(Collider collision)
    {
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
        Gizmos.DrawCube(transform.position, collider.bounds.size);
    }
}
