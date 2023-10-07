using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] bool _shouldDisableAfterTrigger = false;
    [SerializeField] UnityEvent _onTriggerEnterEvent;
    [SerializeField] UnityEvent _onTriggerExitEvent;

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

<<<<<<< Updated upstream:Assets/_Project/Scripts/Trigger Mechanics/SimpleTrigger.cs
    private void OnDrawGizmos()
    {
        if (!_showGizmo)
        {
            return;
        }

        BoxCollider collider = GetComponent<BoxCollider>();

        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(transform.position, collider.bounds.size);
=======
    private void OnTriggerExit(Collider collision)
    {
        _onTriggerExitEvent?.Invoke();

>>>>>>> Stashed changes:Assets/_Project/Scripts/SimpleTrigger.cs
    }
}
