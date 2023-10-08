using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] TMP_Text BGMvolumeTextValue = null;
    [SerializeField] Slider BGMvolumeSlider = null;
    [SerializeField] TMP_Text SFXvolumeTextValue = null;
    [SerializeField] Slider SFXvolumeSlider = null;
    [SerializeField] float defaultVolume = 0.7f;
    [SerializeField] VolumeSettings volumeSettings = null;
    [SerializeField] GameObject confirmationPrompt = null;
    [SerializeField] GameObject noSavedGameDialog = null;

    public void NewGameDialogYes()
    {
        GameManager.Instance.LoadNewGame();
    }

    public void LaodGameDialogYes()
    {
        if (PlayerPrefs.HasKey(GameManager.LAST_LEVEL_SAVE_KEY))
        {
            GameManager.Instance.LoadGameSavedLevel();
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetBGMVolume(float volume)
    {
        volumeSettings.SetMusicVolume(volume);
        AudioManager.Instance.SetupVolumes();
        //AudioListener.volume = volume;
        BGMvolumeTextValue.text = volume.ToString("0.0");
    }

    public void SetSFXVolume(float volume)
    {
        volumeSettings.SetSfxVolume(volume);
        AudioManager.Instance.SetupVolumes();
        SFXvolumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            BGMvolumeSlider.value = defaultVolume;
            BGMvolumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

}
