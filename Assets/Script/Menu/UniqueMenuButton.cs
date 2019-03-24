using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UniqueMenuButtonType
{
    AllEraze,
    OpenOrClose,
    Save,
}

public class UniqueMenuButton : MenuButtonBase
{
    public UniqueMenuButtonType ButtonType;
    public Action OnPush;

    protected override void OnPushed()
    {
        if (OnPush != null)
            OnPush();
    }
}
