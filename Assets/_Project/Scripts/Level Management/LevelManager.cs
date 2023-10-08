using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelKey
{
    public string KeyName;
    public bool Enabled;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private bool _startLevelOnAwake = true;
    [SerializeField] private bool _shouldSetupPlayer = true;
    [SerializeField] private Transform _playerStartPosition;

    [Header("Level BGM")]
    [SerializeField] private float _delayToStartBgm = 3f;
    [SerializeField] private string _bgmName = "gameplay";


    [Header("Level State")]
    public List<LevelKey> LevelKeys = new List<LevelKey>();

    [Header("Events")]
    public UnityEvent OnLevelStarts;
    public UnityEvent OnLevelCompleted;
    public UnityEvent OnLevelFailed;
    public UnityEvent OnLevelStateUpdated;
    public UnityEvent OnLevelKeysUpdated;

    private void OnDisable()
    {
        PlayerController.Instance.OnDeath.RemoveListener(LevelFailed);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PlayerController.Instance.OnDeath.AddListener(LevelFailed);

        if (_startLevelOnAwake)
        {
            StartLevel();
        }
    }

    public void SetupPlayer()
    {
        PlayerController.Instance.gameObject.transform.position = _playerStartPosition.position;
        PlayerController.Instance.gameObject.SetActive(true);
        PlayerController.Instance.ToggleEnabled(true);
    }

    public void StartLevel()
    {
        if(_shouldSetupPlayer)
        {
            SetupPlayer();
        }

        Invoke("StartLevelBGM", _delayToStartBgm);

        OnLevelStarts?.Invoke();
    }

    public void StartLevelBGM()
    {
        AudioManager.Instance.PlayBGM(_bgmName);
    }

    public void CompleteLevel()
    {
        AudioManager.Instance.StopCurrentBGM();
        PlayerController.Instance.ToggleEnabled(false);
        // only toggling of the enabled state of the player is not enough to avoid cases in player takes damage right after level is completed
        // so gonna disable the entire component for now
        // TODO: find a better way
        PlayerController.Instance.GetComponent<CanTakeHits>().enabled = false;


        GameplayUIManager.Instance.OpenPage(GameplayUIManager.GameplayUIPages.LevelCompletePage);

        OnLevelCompleted?.Invoke();
    }

    public void LevelFailed()
    {
        AudioManager.Instance.StopCurrentBGM();
        PlayerController.Instance.ToggleEnabled(false);

        GameplayUIManager.Instance.OpenPage(GameplayUIManager.GameplayUIPages.DeathPage);

        OnLevelFailed?.Invoke();
    }

    public void PlayGameEnding()
    {
        GameplayUIManager.Instance.OpenPage(GameplayUIManager.GameplayUIPages.EndingPage);

        // TODO: maybe play a timeline at the end?
    }

    public LevelKey GetLevelKey(string keyName)
    {
        LevelKey key = null;

        foreach (var k in LevelKeys)
        {
            if (k.KeyName == keyName)
            {
                key = k;
                break;
            }
        }

        return key;
    }

    public LevelKey SetLevelKey(string keyName, bool state)
    {
        var key = GetLevelKey(keyName);

        if (key == null)
        {
            LevelKeys.Add(new LevelKey() { KeyName = keyName, Enabled = state });
        }
        else
        {
            key.Enabled = state;
        }

        OnLevelKeysUpdated?.Invoke();

        return key;
    }

    public void LevelStateUpdated()
    {
        OnLevelStateUpdated?.Invoke();
    }
}
