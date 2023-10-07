using UnityEngine;

public class PlayAudioSFX : MonoBehaviour
{
    [SerializeField] AudioSFX _sfx;

    public void Play()
    {
        AudioManager.Instance.PlaySFX(_sfx);
    }

    public void Play(AudioSFX sfx)
    {
        AudioManager.Instance.PlaySFX(sfx);
    }
    public void Play(AudioClip clip)
    {
        AudioManager.Instance.PlaySFX(clip);
    }
}
