using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Jontacos;

public class PaintController : MonoBehaviour
{
    private static readonly float PULLET_DOWN_Y = -600;
    private static readonly float PULLET_UP_Y = PULLET_DOWN_Y + 120;
    private static readonly float CANVAS_TEX_X = 1820;

    enum ColorType
    {
        Erazer = 0,
        Red,
        Green,
        Blue,
        White,
        Gray
    }

    private class Brush
    {
        public int Width = 8;
        public int Height = 8;
        /// <summary> 現在色保持用 </summary>
        public Color Color = Color.gray;
        /// <summary> ブラシサイズ分の色配列 </summary>
        public Color[] Colors;

        public Brush()
        {
            Colors = new Color[Width * Height];
            for (int i = 0; i < Colors.Length; ++i)
                Colors[i] = Color;
        }

        public void UpdateColor(Color col)
        {
            Color = col;
            for (int i = 0; i < Colors.Length; ++i)
                Colors[i] = Color;
        }
    }
    private Brush brush;

    [SerializeField]
    private EdgeTexture EdgeTex;

    [SerializeField]
    private RawImage WriteRawImage;

    /// <summary>
    /// スクショ保存
    /// </summary>
    private SaveScreenShot saveScreen;

    public GameObject Menus;
    public GameObject ColorPullet;

    public SavedImagePresenter SavedImageScroller;

    private Page writePage;
    private bool isOpendColorPullet = true;
    private bool isMovingPullet = false;
    private bool isUnpaintable = false;

    private Vector2 prePos;

    void Start ()
    {
        brush = new Brush();
        saveScreen = new SaveScreenShot();
        prePos = Vector2.zero;

        CreatePage();
        SavedImageScroller.Initialize();
        SavedImageScroller.gameObject.SetActive(false);
    }
	
	void Update ()
    {
        if (isUnpaintable)
            return;
        // マウス座標をワールド座標からスクリーン座標に変換する
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // マウスクリック
        if (UtilTouch.GetTouch() != TouchInfo.None)
            UpdatePixel(UtilTouch.GetTouchPosition());
        else
            prePos = Vector2.zero;
    }

    /// <summary>
    /// お絵描き禁止にするならtrue
    /// </summary>
    /// <param name="flg"></param>
    public void SetUnPaintable(bool flg)
    {
        isUnpaintable = flg;
    }

    /// <summary>
    /// 書き込み用テクスチャの再生成
    /// </summary>
    public void CreatePage()
    {
        writePage = new Page();
        writePage.SetCanvas(WriteRawImage);
    }

    /// <summary>
    /// 保存済み画像をスクロールで表示
    /// </summary>
    public void OpenImageScroller()
    {
        if (!SavedImageScroller.IsOpend)
            SavedImageScroller.Open();
        else
            SavedImageScroller.Close();
    }

    public void SetEdgeTexture(Texture2D tex)
    {
        EdgeTex.SetTextureByAspect(tex);
    }

    /// <summary>
    /// テクスチャへの書き込み
    /// </summary>
    /// <param name="position"></param>
    private void UpdatePixel(Vector2 position)
    {
        var rx = position.x / (Screen.width * CANVAS_TEX_X / 1920) * writePage.TexWidth;
        var ry = position.y / Screen.height * writePage.TexHeight;

        if (rx < 0 || rx > writePage.TexWidth || ry < 0 || ry > writePage.TexHeight)
        {
            prePos = Vector2.zero;
            return;
        }

        if (prePos.sqrMagnitude > 0.5f)
        {
            var pos = new Vector2(rx, ry);
            var nor = (pos - prePos).normalized;
            int len = (int)Mathf.Abs(Vector2.Distance(pos, prePos));
            var dis = Vector2.zero;
            for (int i = 0; i < len; ++i)
            {
                dis += nor;
                writePage.WriteCircleNotApply((int)(rx + dis.x), (int)(ry + dis.y), brush.Color, brush.Width);
            }
        }

        writePage.WriteCircle((int)rx, (int)ry, brush.Color, brush.Width);
        prePos = new Vector2(rx, ry);
    }

    public void ChangeScale(int addScl)
    {
        brush.Width = Mathf.Max(1, brush.Width + addScl);
    }
    public void ColorChange(Color col)
    {
        brush.UpdateColor(col);
    }

    /// <summary>
    /// カラーパレットのON/OFF
    /// </summary>
    public void OpenOrCloseColorPullet()
    {
        if (isMovingPullet)
            return;

        isMovingPullet = true;
        if (isOpendColorPullet)
            StartCoroutine(OpenColorPullet(0.5f));
        else
            StartCoroutine(CloseColorPullet(0.5f));
    }
    private IEnumerator OpenColorPullet(float time)
    {
        isUnpaintable = true;
        var t = 0f;
        while (t < time)
        {
            var y = Utils.EaseOut(PULLET_DOWN_Y, PULLET_UP_Y, t,time);
            ColorPullet.transform.SetLocalPositionY(y);
            t += Time.deltaTime;
            yield return null;
        }
        isMovingPullet = false;
        isOpendColorPullet = false;
    }
    private IEnumerator CloseColorPullet(float time)
    {
        var t = 0f;
        while (t < time)
        {
            var y = Utils.EaseOut(PULLET_UP_Y, PULLET_DOWN_Y, t, time);
            ColorPullet.transform.SetLocalPositionY(y);
            t += Time.deltaTime;
            yield return null;
        }
        isMovingPullet = false;
        isOpendColorPullet = true;
        isUnpaintable = false;
    }

    public void OnSaveButton()
    {
        StartCoroutine(SaveScreenShot());
    }
    public void OnChangeViewActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private IEnumerator SaveScreenShot()
    {
        Menus.SetActive(false);

        yield return StartCoroutine(saveScreen.Save(EdgeTex));

        Menus.SetActive(true);
    }

}
