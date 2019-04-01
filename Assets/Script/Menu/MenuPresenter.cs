using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        View.SetAllErazeButton(Painter.CreatePage);
        View.SetPulletOpenOrCloseMenuButton(Painter.OpenOrCloseColorPullet);
        View.SetSaveButton(Painter.OnSaveButton);
        View.SetChangeViewButton(Painter.OnChangeViewActive);
        View.SetOpenImageScroller(Painter.OpenImageScroller);
    }
}
