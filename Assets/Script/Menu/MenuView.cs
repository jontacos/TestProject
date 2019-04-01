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

    /// <summary>
    /// メインメニュー用ボタンの親キャンバス
    /// </summary>
    [SerializeField]
    private GameObject MainMenuButtonCanvas;

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

    #region UniqueButtons
    public void SetAllErazeButton(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.AllEraze);
        btn.OnPush = OnPush;
    }
    public void SetPulletOpenOrCloseMenuButton(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.PulletMove);
        btn.OnPush = OnPush;
    }
    public void SetSaveButton(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.Save);
        btn.OnPush = OnPush;
    }
    public void SetChangeViewButton(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.ChangeView);
        btn.OnPush = OnPush;
        btn.OnPush += () => { MainMenuButtonCanvas.SetActive(!MainMenuButtonCanvas.activeSelf); };
    }
    public void SetOpenImageScroller(Action OnPush)
    {
        var btn = GetUniqueButton(UniqueMenuButtonType.OpenScroller);
        btn.OnPush = OnPush;
    }
    #endregion

    private UniqueMenuButton GetUniqueButton(UniqueMenuButtonType type)
    {
        return UniqueButtons.FirstOrDefault(b => b.ButtonType == type);
    }
}
