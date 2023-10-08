using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueStarter : MonoBehaviour
{
    public DialogueSequenceSO sequence;
    public bool ignoreIfPlayingTheSame = true;

    public UnityEvent OnSequenceStart;
    public UnityEvent OnSequenceEnd;

    public void PlaySequence()
    {
        OnSequenceStart?.Invoke();

        if (ignoreIfPlayingTheSame)
        {
            if (DialogueController.Instance.CurrentSequence == sequence) return;
        }

        DialogueController.Instance.PlaySequence(sequence, OnSequenceEnd);
    }
}
