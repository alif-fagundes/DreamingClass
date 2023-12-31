using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance;

    [SerializeField] private CanvasGroup dialoguePanel;
    [SerializeField] private TextMeshProUGUI text;
    public bool IsPlaying = false;


    [Space(10)]
    public UnityEvent OnDialogueStart;
    public UnityEvent OnDialogueEnd;

    public DialogueSequenceSO CurrentSequence;
    private int conversationIndex = 0;

    private UnityEvent _onSequenceEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void PlaySequence(DialogueSequenceSO sequence, UnityEvent onSequenceEnd = null)
    {
        conversationIndex = 0;
        CurrentSequence = sequence;
        IsPlaying = true;
        _onSequenceEnd = onSequenceEnd;
        StartCoroutine(HandleDialogueStart());
    }

    public IEnumerator HandleDialogueStart()
    {
        yield return new WaitForSeconds(CurrentSequence.dialogue[conversationIndex].delayToStart);

        SetupContent();
        dialoguePanel.alpha = 1;
        OnDialogueStart?.Invoke();
        StartCoroutine(HandleDialogueEnd());
        yield return null;
    }

    public void End()
    {
        _onSequenceEnd?.Invoke();
        conversationIndex = 0;
        IsPlaying = false;
    }

    private IEnumerator HandleDialogueEnd()
    {
        yield return new WaitForSeconds(CurrentSequence.dialogue[conversationIndex].delayToEnd);
        dialoguePanel.alpha = 0;
        conversationIndex++;

        if (conversationIndex >= CurrentSequence.dialogue.Length)
        {
            End();
        }
        else
        {
            StartCoroutine(HandleDialogueStart());
        }

        yield return null;
    }

    private void SetupContent()
    {
        text.text = CurrentSequence.dialogue[conversationIndex].text;
    }
}
