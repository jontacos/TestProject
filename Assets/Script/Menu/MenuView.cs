using Jontacos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    /// <summary>
    /// パレットの移動量
    /// </summary>
    private static readonly float PULLET_MOVE_Y = 120;

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

    /// <summary>
    /// 色変更などのパレット並べる用レイアウトグループ
    /// </summary>
    [SerializeField]
    private GameObject ColorPulletParent;

    /// <summary>
    /// パレット移動用ボタンのテキスト
    /// </summary>
    [SerializeField]
    private Text PulletMoveButtonText;

    private int ColorPulletMoveInHash = Animator.StringToHash("UIColorPulletAnimation");
    private int ColorPulletMoveOutHash = Animator.StringToHash("ReturnUIColorPulletAnimation");

    /// <summary>
    /// パレット移動中かどうか
    /// </summary>
    private bool isMovingPullet = false;

    /// <summary>
    /// パレット開いた状態かどうか
    /// </summary>
    public bool IsOpendColorPullet { get; private set; }


    public Animator ColorPulletAnime;

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

    public void SetUniqueButton(Action OnPush, UniqueMenuButtonType Type)
    {
        var btn = GetUniqueButton(Type);
        btn.OnPush = OnPush;
    }

    private UniqueMenuButton GetUniqueButton(UniqueMenuButtonType type)
    {
        return UniqueButtons.FirstOrDefault(b => b.ButtonType == type);
    }


    public IEnumerator OpenColorPullet(float time)
    {
        if (isMovingPullet)
            yield break;
        isMovingPullet = true;
        ColorPulletAnime.Play(ColorPulletMoveInHash, 0, 0);
        //normarizedTimeの0化は1F後におこなわれるので一旦待つ
        yield return null;

        while (ColorPulletAnime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;
        isMovingPullet = false;
        IsOpendColorPullet = true;
        PulletMoveButtonText.text = "▽";
    }
    public IEnumerator CloseColorPullet(float time)
    {
        if (isMovingPullet)
            yield break;
        isMovingPullet = true;
        ColorPulletAnime.Play(ColorPulletMoveOutHash, 0, 0);
        //normarizedTimeの0化は1F後におこなわれるので一旦待つ
        yield return null;

        while (ColorPulletAnime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;
        isMovingPullet = false;
        IsOpendColorPullet = false;
        PulletMoveButtonText.text = "△";
    }
}
