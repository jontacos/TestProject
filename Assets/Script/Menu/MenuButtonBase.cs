using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuButtonBase : MonoBehaviour
{
    protected abstract void OnPushed();

    protected virtual void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnPushed);
    }
}
