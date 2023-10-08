using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelKey
{
    public string keyName;
    public bool state;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private bool _startLevelOnAwake = true;

    [Header("Level State")]
    public LevelKey[] LevelKeys;

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
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(_startLevelOnAwake)
        {
            StartLevel();
        }

        PlayerController.Instance.OnDeath.AddListener(LevelFailed);
    }

    public void StartLevel()
    {


        OnLevelStarts?.Invoke();
    }

    public void CompleteLevel()
    {


        OnLevelCompleted?.Invoke();
    }

    public void LevelFailed()
    {


        OnLevelFailed?.Invoke();
    }

    public LevelKey CheckLevelKey(string keyName)
    {
        LevelKey key = null;

        foreach(var k in LevelKeys)
        {
            if(k.keyName == keyName)
            {
                key = k;
                break;
            }
        }

        return key;
    }

    public void LevelStateUpdate()
    {
        OnLevelStateUpdated?.Invoke();
    }
}
