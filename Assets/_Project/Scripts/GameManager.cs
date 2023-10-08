using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string MainMenuScene = "MainMenu";
    public string[] Levels;

    public int CurrentLevel = 0;

    [Header("Events")]
    public UnityEvent OnLevelLoadingStarts;
    public UnityEvent OnLevelLoadingFinish;

    public const string  LAST_LEVEL_SAVE_KEY = "LAST_LEVEL";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGameSavedLevel()
    {
        var loaded = PlayerPrefs.GetInt(LAST_LEVEL_SAVE_KEY, 0);
        CurrentLevel = loaded;
        StartCoroutine(LoadSceneAsync(Levels[CurrentLevel]));
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt(LAST_LEVEL_SAVE_KEY, CurrentLevel);
    }

    public void LoadMainMenu()
    {
        CurrentLevel = 0;
        StartCoroutine(LoadSceneAsync(MainMenuScene));
    }

    public void LoadNewGame()
    {
        CurrentLevel = 0;
        StartCoroutine(LoadSceneAsync(Levels[CurrentLevel]));
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;
        StartCoroutine(LoadSceneAsync(Levels[CurrentLevel]));
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadSceneAsync(Levels[CurrentLevel]));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {

        OnLevelLoadingStarts?.Invoke();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1.5f);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Scene has loaded, so allow it to activate
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        OnLevelLoadingFinish?.Invoke();
    }
}
