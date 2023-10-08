using UnityEngine;

public class LevelKeyUpdater : MonoBehaviour
{
    [SerializeField] private string keyName;

    public void EnableKey()
    {
        LevelManager.Instance.SetLevelKey(keyName, true);
    }

    public void DisableKey()
    {
        LevelManager.Instance.SetLevelKey(keyName, false);
    }
}
