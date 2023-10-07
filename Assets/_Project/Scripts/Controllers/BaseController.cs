using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public bool IsEnabled;
    protected Action<Boolean> ToggleEnabledCallback = null;

    public void ToggleEnabled(bool value)
    {
        IsEnabled = value;

        ToggleEnabledCallback?.Invoke(value);
    }
}
