using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialogue/Sequence", fileName = "New_DialogueSequence")]

public class DialogueSequenceSO : ScriptableObject
{
    public Dialogue[] dialogue;
}
