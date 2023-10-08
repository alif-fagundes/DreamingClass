using System;
using UnityEngine;

[Serializable]
public class UIPage<T> where T : Enum
{
    public T pageID;
    public CanvasGroup canvasGroup;
    public bool ShouldPauseGame = true;
}

public class GameplayUIManager : MonoBehaviour
{
    [Serializable]
    public enum GameplayUIPages
    {
        None = 0,
        PausePage = 1,
        OptionsPage = 2,
        DeathPage = 3,
        LevelCompletePage = 4,
        EndingPage = 5,
    }

    public static GameplayUIManager Instance;

    [SerializeField] private GameplayUIPages _startsWith = GameplayUIPages.None;
    [SerializeField] private CanvasGroup _background;
    public UIPage<GameplayUIPages>[] _pages;

    private UIPage<GameplayUIPages> _currentActivePage = null;

    private InputManager _input;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _input = FindAnyObjectByType<InputManager>();

        if(_input == null)
        {
            Debug.LogError("Scene is missing a inputManager, check Player");
        }
    }

    private void Start()
    {
        if (_startsWith != GameplayUIPages.None)
        {
            OpenPage(_startsWith);
        }
    }

    private void Update()
    {
        if (_input.Pause)
        {
            _input.Pause = false;

            if (GameManager.Instance.GameState == GameManager.EGameState.Playing)
            {
                // pause

                OpenPage(GameplayUIPages.PausePage);
            }
        }
    }

    public void NextLevelButton()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void ResumeButton()
    {
        CloseActivePage();
    }

    public void QuitButton()
    {
        GameManager.Instance.QuitGame();
    }

    public void MainMenuButton()
    {
        GameManager.Instance.LoadMainMenu();
    }

    #region Page Stuff
    public void OpenPage(int page)
    {
        // open page based on enum value
        OpenPage((GameplayUIPages)page);
    }

    public void OpenPage(GameplayUIPages page)
    {
        if (_currentActivePage != null)
        {
            CloseActivePage();
        }

        foreach (var p in _pages)
        {
            if (p.pageID == page)
            {
                TogglePage(p, true);
                ToggleBackground(true);
                _currentActivePage = p;

                if (p.ShouldPauseGame)
                {
                    GameManager.Instance.PauseGame();
                }

                break;
            }
        }
    }

    public void CloseActivePage()
    {
        TogglePage(_currentActivePage, false);
        ToggleBackground(false);
        _currentActivePage = null;

        GameManager.Instance.ResumeGame();
    }

    private void TogglePage(UIPage<GameplayUIPages> page, bool value)
    {
        // TODO: animate smoothly the alpha to fade in/out 
        if (value)
        {
            page.canvasGroup.alpha = 1f;
            page.canvasGroup.blocksRaycasts = true;
            page.canvasGroup.interactable = true;
        }
        else
        {
            page.canvasGroup.alpha = 0f;
            page.canvasGroup.blocksRaycasts = false;
            page.canvasGroup.interactable = false;
        }
    }

    private void ToggleBackground(bool value)
    {
        // TODO: animate smoothly the alpha to fade in/out 
        if (value)
        {
            _background.alpha = 1f;
            _background.blocksRaycasts = true;
            _background.interactable = true;
        }
        else
        {
            _background.alpha = 0f;
            _background.blocksRaycasts = false;
            _background.interactable = false;
        }
    }
    #endregion Page Stuff
}
