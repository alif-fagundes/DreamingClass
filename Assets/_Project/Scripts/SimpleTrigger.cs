using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] bool _shouldDisableAfterTrigger = false;
    [SerializeField] UnityEvent _onTriggerEnterEvent;

    private void OnTriggerEnter(Collider collision)
    {
        _onTriggerEnterEvent?.Invoke();

        if (_shouldDisableAfterTrigger)
        {
            gameObject.SetActive(false);    
        }
    }
}
