using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [TextArea] public string text;
    public float delayToStart = 0;
    public float delayToEnd = 2f;
}
