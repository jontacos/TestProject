using Jontacos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPresenter : MonoBehaviour
{
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
        View.SetUniqueButton(PulletMove, UniqueMenuButtonType.PulletMove);
        View.SetUniqueButton(Painter.OnSaveButton, UniqueMenuButtonType.Save);
        View.SetUniqueButton(Painter.OpenImageScroller, UniqueMenuButtonType.OpenScroller);
        //View.SetUniqueButton(ChangeScene, UniqueMenuButtonType.ChangeView);
    }

    private void PulletMove()
    {
        StartCoroutine(PulletMoceCoroutine());
    }

    private IEnumerator PulletMoceCoroutine()
    {
        if (!View.IsOpendColorPullet)
        {
            Painter.IsUnpaintable = true;
            yield return StartCoroutine(View.OpenColorPullet(0.5f));
        }
        else
        {
            yield return StartCoroutine(View.CloseColorPullet(0.5f));
            Painter.IsUnpaintable = false;
        }
    }
}
