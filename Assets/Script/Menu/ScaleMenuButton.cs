using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleMenuButton : MenuButtonBase
{
    private enum ScaleIndex
    {
        Shrink = -1,
        Expand = 1,
    }

    [SerializeField]
    private ScaleIndex scale;

    public Action<int> OnPush;

 //   protected override void Start ()
 //   {
		
	//}

    protected override void OnPushed()
    {
        if(OnPush != null)
            OnPush((int)scale);
    }
}
