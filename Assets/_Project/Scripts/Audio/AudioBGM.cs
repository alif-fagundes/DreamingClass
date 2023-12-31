﻿using System;
using UnityEngine;

[Serializable]
public class AudioBGM
{
    public string name;
    public AudioClip clip;
    public bool loop;
    [Range(0f, 1f)]
    public float volume;
}
