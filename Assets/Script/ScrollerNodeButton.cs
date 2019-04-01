using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerNodeButton : MenuButtonBase
{
    public Action<Texture2D> OnPush;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    protected override void OnPushed()
    {
        if (OnPush != null)
            OnPush(image.sprite.texture);
    }
}
