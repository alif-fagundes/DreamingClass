using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay;

    public void DelayedDisable()
    {
        Invoke("Disable", delay);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
