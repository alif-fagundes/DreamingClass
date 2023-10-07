using UnityEngine;

public class SimpleDamageDealer : MonoBehaviour, IDoDamage
{
    [SerializeField] int _damage = 5;

    public void DoDamage(ITakeDamage other)
    {
        other.TakeDamage(_damage);
    }
}
