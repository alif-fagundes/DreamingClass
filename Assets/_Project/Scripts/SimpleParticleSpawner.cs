using UnityEngine;

public class SimpleParticleSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void Spawn()
    {
        Instantiate(_particleSystem, transform.position, Quaternion.identity);
    }
}
