using UnityEngine;

public class PlayAudioSFX : MonoBehaviour
{
    [SerializeField] private AudioSFX _sfx;
    [SerializeField] private AudioSource _source;
    [SerializeField] private bool _shouldUseOwnSource = false;

    public void Play()
    {
        AudioManager.Instance.PlaySFX(_sfx);
    }

    public void Play(AudioSFX sfx)
    {
        if (_shouldUseOwnSource)
        {
            AudioManager.Instance.PlaySFXWithSpecialSource(sfx, _source);
        }
        else
        {
            AudioManager.Instance.PlaySFX(sfx);
        }

    }

    public void Play(AudioClip clip)
    {
        AudioManager.Instance.PlaySFX(clip);
    }
}
