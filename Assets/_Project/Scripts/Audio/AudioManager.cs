using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    [SerializeField] AudioSource[] _SFXSources;

    [Space(5)]
    [Header("BMG")]
    [SerializeField] AudioSource _BGMSource;
    [SerializeField] bool _playOnStart = true;
    [SerializeField] AudioBGM[] _availableBGM;


    [Space(5)]
    [Header("Setup Volumes")]
    public AudioMixer mixer;
    [Range(-80f, 2f)] public float BGMVolume = 1f;
    [Range(-80f, 2f)] public float SFXVolume = 1f;
    [SerializeField] bool _shouldDebug = false;
    [SerializeField] AudioSFX _testSFX;

    Queue<AudioSource> _playedSFXSources = new Queue<AudioSource>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (_playOnStart)
        {
            PlayBGM();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        AudioSource sourceToUse = null;

        for (int i = 0; i < _SFXSources.Length; i++)
        {
            // look for available audio sources

            if (!_SFXSources[i].isPlaying)
            {
                // found available audio source
                sourceToUse = _SFXSources[i];
                _playedSFXSources.Enqueue(sourceToUse);
                break;
            }
        }

        if (sourceToUse == null)
        {
            // did not found an available audio source so use the last one played
            sourceToUse = _playedSFXSources.Dequeue();
        }

        if (sourceToUse != null)
        {
            sourceToUse.clip = clip;
            sourceToUse.volume = volume;
            sourceToUse.pitch = pitch;

            sourceToUse.Play();
        }
        else
        {
            Debug.LogError("No AudioSource found for SFXs");
        }
    }

    public void PlaySFX(AudioSFX sfx)
    {
        PlaySFX(sfx.clips[UnityEngine.Random.Range(0, sfx.clips.Length)], sfx.volume, UnityEngine.Random.Range(sfx.pitchVariation.x, sfx.pitchVariation.y));
    }

    public void PlayBGM()
    {
        PlayBGM(_availableBGM[0]);
    }

    public void PlayBGM(string _audioName)
    {
        foreach (var _a in _availableBGM)
        {
            if (_a.name == _audioName)
            {
                _BGMSource.clip = _a.clip;
                _BGMSource.loop = _a.loop;
                _BGMSource.volume = _a.volume;

                _BGMSource.Play();
            }
        }
    }

    public void PlayBGM(AudioBGM _audioBGItem)
    {
        _BGMSource.clip = _audioBGItem.clip;
        _BGMSource.loop = _audioBGItem.loop;
        _BGMSource.volume = _audioBGItem.volume;

        _BGMSource.Play();
    }

    public void StopCurrentBGM()
    {
        _BGMSource.Stop();
    }

    public void SetupVolumes()
    {
        mixer.SetFloat("BGMVolume", BGMVolume);
        mixer.SetFloat("SFXVolume", SFXVolume);
    }

    private void OnGUI()
    {
        if(!_shouldDebug) return;

        // Define the button's position and size.
        Rect buttonRect = new Rect(10, 10, 150, 50);

        // Create a button with the specified position, size, and label.
        if (GUI.Button(buttonRect, "Test SFX"))
        {
            // Called when the button is clicked.
            PlaySFX(_testSFX);
        }

        buttonRect = new Rect(10, 60, 150, 50);
        if (GUI.Button(buttonRect, "Update Volumes"))
        {
            SetupVolumes();
        }
    }
}
