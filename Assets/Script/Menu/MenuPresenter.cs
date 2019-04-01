using Jontacos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPresenter : MonoBehaviour
{
    /// <summary>
    /// パレットの移動量
    /// </summary>
    private static readonly float PULLET_MOVE_Y = 120;

    /// <summary>
    /// パレット移動中かどうか
    /// </summary>
    private bool isMovingPullet = false;
    /// <summary>
    /// パレット開いた状態かどうか
    /// </summary>
    private bool isOpendColorPullet = false;

    /// <summary>
    /// 色変更などのパレット並べるようレイアウトグループ
    /// </summary>
    [SerializeField]
    private GameObject ColorPulletParent;

    /// <summary>
    /// パレット移動用ボタンのテキスト
    /// </summary>
    [SerializeField]
    private Text PulletMoveButtonText;

    public MenuView View;
    public PaintController Painter;

    void Start ()
    {
        SetColorButtonPush();
        SetScaleButtonPush();
        SetUniqueButtonPush();
    }
	
    void SetColorButtonPush()
    {
        View.SetColorButtonListener(Painter.ColorChange);
    }
    void SetScaleButtonPush()
    {
        View.SetScaleButtonListener(Painter.ChangeScale);
    }
    void SetUniqueButtonPush()
    {
        View.SetUniqueButton(Painter.CreatePage, UniqueMenuButtonType.AllEraze);
        View.SetUniqueButton(OpenOrCloseColorPullet, UniqueMenuButtonType.PulletMove);
        View.SetUniqueButton(Painter.OnSaveButton, UniqueMenuButtonType.Save);
        View.SetUniqueButton(Painter.OpenImageScroller, UniqueMenuButtonType.OpenScroller);
        //View.SetUniqueButton(Painter.OnChangeViewActive, UniqueMenuButtonType.ChangeView);
    }


    /// <summary>
    /// カラーパレットのON/OFF
    /// </summary>
    public void OpenOrCloseColorPullet()
    {
        if (isMovingPullet)
            return;

        isMovingPullet = true;
        if (!isOpendColorPullet)
            StartCoroutine(OpenColorPullet(0.5f));
        else
            StartCoroutine(CloseColorPullet(0.5f));
    }
    private IEnumerator OpenColorPullet(float time)
    {
        Painter.IsUnpaintable = true;
        var t = 0f;
        var start = ColorPulletParent.transform.position.y;
        var goal = ColorPulletParent.transform.position.y + PULLET_MOVE_Y;
        while (t < time)
        {
            int y = (int)Utils.EaseOut(start, goal, t, time);
            ColorPulletParent.transform.SetPositionY(y);
            t += Time.deltaTime;
            yield return null;
        }
        isMovingPullet = false;
        isOpendColorPullet = true;
        PulletMoveButtonText.text = "▽";
    }
    private IEnumerator CloseColorPullet(float time)
    {
        var t = 0f;
        var start = ColorPulletParent.transform.position.y;
        var goal = ColorPulletParent.transform.position.y - PULLET_MOVE_Y;
        while (t < time)
        {
            int y = (int)Utils.EaseOut(start, goal, t, time);
            ColorPulletParent.transform.SetPositionY(y);
            t += Time.deltaTime;
            yield return null;
        }
        isMovingPullet = false;
        isOpendColorPullet = false;
        Painter.IsUnpaintable = false;
        PulletMoveButtonText.text = "△";
    }

}
