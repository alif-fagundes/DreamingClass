using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Text BGMvolumeTextValue = null;
    [SerializeField] TMP_Text SFXvolumeTextValue = null;
    //[SerializeField] Slider musicSlider;
    //[SerializeField] Slider SfxSlider;


    /*
    private void Start()
    {
        GetMusicVolume();
        GetSfxVolume();
    }
    */

        public void SetMusicVolume(float volume)
        {
            float _volume;
            if (volume == 0) _volume = -80;
            else _volume = Mathf.Log10(volume) * 20;

            audioMixer.SetFloat("MusicVolume", _volume);
            AudioManager.Instance.BGMVolume = _volume;
            AudioManager.Instance.SetupVolumes();
            BGMvolumeTextValue.text = volume.ToString("0.0");
        }

        public void SetSfxVolume(float volume)
        {
            float _volume;
            if (volume == 0) _volume = -80;
            else _volume = Mathf.Log10(volume) * 20;
            //float _volume = (volume * (1 + 80)) - 80;

            audioMixer.SetFloat("SfxVolume", _volume);
            AudioManager.Instance.SFXVolume = _volume;
            AudioManager.Instance.SetupVolumes();
            SFXvolumeTextValue.text = volume.ToString("0.0");
        }

        public void SetVolumeMaster(float volume)
        {
            SetMusicVolume(volume);
            SetSfxVolume(volume);
        }

    /*
        public void GetMusicVolume()
        {
            musicSlider.value = AudioManager.Instance.BGMVolume;
            float volume = musicSlider.value;
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        public void GetSfxVolume()
        {
            SfxSlider.value = AudioManager.Instance.SFXVolume;
            float volume = SfxSlider.value;
            audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
        }
    */

}

