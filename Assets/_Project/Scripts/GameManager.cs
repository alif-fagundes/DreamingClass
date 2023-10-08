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

    public void LoadGame(string SavedData)
    {
        // TODO
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
