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

    [SerializeField] private float _delayToStartBgm = 3f;
    [SerializeField] private string _bgmName = "gameplay";

    [SerializeField] private bool _startLevelOnAwake = true;
    [SerializeField] private Transform _playerStartPosition;

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

    public void StartLevel()
    {
        PlayerController.Instance.ToggleEnabled(true);

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

        OnLevelCompleted?.Invoke();
    }

    public void LevelFailed()
    {
        AudioManager.Instance.StopCurrentBGM();
        PlayerController.Instance.ToggleEnabled(false);

        // TODO: turn on game over screen

        OnLevelFailed?.Invoke();
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
