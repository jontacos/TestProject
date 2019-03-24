using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    private ColorMenuButton[] ColorButtons;
    private ScaleMenuButton[] ScaleButtons;

    /// <summary>
    /// 固有機能を持ったメニューボタン
    /// </summary>
    [SerializeField]
    private UniqueMenuButton[] UniqueButtons;

    void Start ()
    {
        ColorButtons = GetComponentsInChildren<ColorMenuButton>();
        ScaleButtons = GetComponentsInChildren<ScaleMenuButton>();
        UniqueButtons = GetComponentsInChildren<UniqueMenuButton>();
    }

    public void SetColorButtonListener (Action<Color> OnPush)
    {
        foreach (var cb in ColorButtons)
            cb.OnPush = OnPush;
    }
    public void SetScaleButtonListener(Action<int> OnPush)
    {
        foreach (var sb in ScaleButtons)
            sb.OnPush = OnPush;
    }

    public void SetAllErazeButton(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.AllEraze);
        btn.OnPush = OnPush;
    }
    public void SetOpenOrCloseMenuButton(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.OpenOrClose);
        btn.OnPush = OnPush;
    }
    public void SetSaveButton(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.Save);
        btn.OnPush = OnPush;
    }
    private UniqueMenuButton GetUniqueButton(UniqueMenuButtonType type)
    {
        return UniqueButtons.FirstOrDefault(b => b.ButtonType == type);
    }
}
