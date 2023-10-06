using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioBGItem
{
    public string name;
    public AudioClip clip;
    public bool loop;
    [Range(0f, 1f)]
    public float volume;
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [Header("SFX")]
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    [SerializeField] AudioSource source3;

    [SerializeField] AudioItem[] audios;

    [Space(5)]
    [Header("BMG")]
    [SerializeField] AudioSource sourceBG;
    [SerializeField] AudioBGItem[] audiosBG;


    [Space(5)]
    [Header("Setup Volumes")]
    public float volumeMasterMusic = 1f;
    public float volumeMasterSfx = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayAudio(string _audioName)
    {
        AudioSource _curSource;

        if (source1.isPlaying)
        {
            if (source2.isPlaying)
            {
                if (source3.isPlaying)
                {
                    _curSource = source1;
                }
                else
                {
                    _curSource = source3;
                }
            }
            else
            {
                _curSource = source2;
            }
        }
        else
        {
            _curSource = source1;
        }

        foreach (var _a in audios)
        {
            if (_a.name == _audioName)
            {
                _curSource.clip = _a.clip;
                _curSource.volume = _a.volume;
                _curSource.pitch = UnityEngine.Random.Range(_a.pitchVariation.x, _a.pitchVariation.y);

                _curSource.Play();
            }
        }
    }

    public void PlayAudioBG(string _audioName)
    {
        AudioSource _curSource = sourceBG;        

        foreach (var _a in audiosBG)
        {
            if (_a.name == _audioName)
            {
                _curSource.clip = _a.clip;
                _curSource.loop = _a.loop;
                _curSource.volume = _a.volume;

                _curSource.Play();
            }
        }
    }

    public void PlayAudioBG(AudioBGItem _audioBGItem)
    {
        AudioSource _curSource = sourceBG;

        _curSource.clip = _audioBGItem.clip;
        _curSource.loop = _audioBGItem.loop;
        _curSource.volume = _audioBGItem.volume;

        _curSource.Play();
    }

    public void StopAudioBG()
    {
        sourceBG.Stop();
    }

    [Serializable]
    public class AudioItem
    {
        public string name;
        public AudioClip clip;
        [Range(0f,1f)]
        public float volume;
        public Vector2 pitchVariation;
    }
}
