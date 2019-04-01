using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Jontacos;

public class PaintController : MonoBehaviour
{
    /// <summary>
    /// 横1920のとき、書き込みテクスチャのサイズは1820にするので
    /// その割合を出しておく
    /// </summary>
    private static readonly float WRITE_ASPECT_RATIO_X = 1820f / 1920f;

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
    //public GameObject ColorPullet;

    public SavedImagePresenter SavedImageScroller;

    private Page writePage;

    private Vector2 prePos;

    /// <summary>
    /// 書き込み不可状態かどうか
    /// </summary>
    public bool IsUnpaintable { get; set; }

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
        if (IsUnpaintable)
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
        var rx = position.x / (Screen.width * WRITE_ASPECT_RATIO_X) * writePage.TexWidth;
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
