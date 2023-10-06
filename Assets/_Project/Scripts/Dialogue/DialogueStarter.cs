using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueStarter : MonoBehaviour
{
    public DialogueSequenceSO sequence;

    public UnityEvent OnSequenceStart;
    public UnityEvent OnSequenceEnd;

    public void PlaySequence()
    {
        OnSequenceStart?.Invoke();

        DialogueController.Instance.PlaySequence(sequence, OnSequenceEnd);
    }
}
