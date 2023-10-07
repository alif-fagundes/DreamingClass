using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "new_SFX", menuName = "Audio/SFX")]
public class AudioSFX : ScriptableObject
{
    public AudioClip[] clips;
    [Range(0f, 1f)]
    public float volume;
    public Vector2 pitchVariation;
}
