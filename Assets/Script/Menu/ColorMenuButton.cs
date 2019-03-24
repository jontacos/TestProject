using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMenuButton : MenuButtonBase
{
    [SerializeField]
    private Color color;

    public Action<Color> OnPush;

    //protected override void Start ()
    //{
    //    var button = GetComponent<Button>();
    //    button.onClick.AddListener(OnPushed);
    //}
	
    protected override void OnPushed()
    {
        if (OnPush != null)
            OnPush(color);
    }
}
