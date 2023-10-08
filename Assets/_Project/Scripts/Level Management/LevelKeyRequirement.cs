using UnityEngine;
using UnityEngine.Events;

public class LevelKeyRequirement : MonoBehaviour
{
    [SerializeField] private string[] levelKeyRequirements;
    [SerializeField] private bool shouldStayCompleted = true;
    public bool IsCompleted = false;

    [Header("Events")]
    public UnityEvent OnCompleted;
    public UnityEvent OnCompletionUndone;

    private void OnDisable()
    {
        LevelManager.Instance.OnLevelKeysUpdated.RemoveListener(CheckCompletion);
    }

    private void Start()
    {
        LevelManager.Instance.OnLevelKeysUpdated.AddListener(CheckCompletion);
    }

    public void CheckCompletion()
    {
        if(shouldStayCompleted && IsCompleted)
        {
            return;
        }

        var allCompleted = true;
        foreach (var requirement in levelKeyRequirements)
        {
            if (!(LevelManager.Instance.GetLevelKey(requirement) != null && LevelManager.Instance.GetLevelKey(requirement).Enabled))
            {
                // found a requirement that isnt enabled

                allCompleted = false;
                break;
            }
        }

        if (allCompleted && !IsCompleted)
        {
            IsCompleted = true;
            OnCompleted?.Invoke();
            LevelManager.Instance.LevelStateUpdated();
        }
        else if (!allCompleted && IsCompleted)
        {
            IsCompleted = false;
            OnCompletionUndone?.Invoke();
        }
    }
}
