using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureWriter : MonoBehaviour
{

    public Texture2D Tex4Write;
    public Material Mat;
    public RawImage WriteRawImage;
    public RawImage EdgeTex;

    public GameObject Pointer;
    public GameObject View;
    private Texture2D copiedTex;

    private class Brush
    {
        private static Color[] ColorTypes = new Color[] { Color.clear, Color.red, Color.green, Color.blue, Color.yellow, Color.gray };

        public int Width = 8;
        public int Height = 8;
        public Color Color = Color.gray;
        public Color[] Colors;

        public Brush()
        {
            Colors = new Color[Width * Height];
            for (int i = 0; i < Colors.Length; ++i)
                Colors[i] = Color;
        }

        public void UpdateColor(int val)
        {
            Color = ColorTypes[val];
            for (int i = 0; i < Colors.Length; ++i)
                Colors[i] = Color;
        }
    }
    private Brush brush;


	void Start ()
    {
        brush = new Brush();
        Mat = View.GetComponent<MeshRenderer>().material;
        copiedTex = new Texture2D(Tex4Write.width, Tex4Write.height, TextureFormat.ARGB32, false);
        copiedTex.LoadRawTextureData(Tex4Write.GetRawTextureData());
    }
	
	void Update ()
    {
        // マウス座標をワールド座標からスクリーン座標に変換する
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(Input.mousePosition);
        Pointer.transform.position = new Vector3(
            mouse.x,
            mouse.y,
            transform.position.z /* マウスのz座標は-10となってしまうため、
            スクリプトがアタッチされているオブジェクトのz座標で補正する */
        );
        // マウスクリック
        if (Jontacos.UtilTouch.GetTouch() != Jontacos.TouchInfo.None)
        {
            UpdatePixel(Jontacos.UtilTouch.GetTouchPosition());
        }
    }

    enum ColorType
    {
        Erazer = 0,
        Red,
        Green,
        Blue,
        White,
        Gray
    }
    public void ColorChange(int value)
    {
        brush.UpdateColor(value);
    }


    // 描き込み(Textureに描き込むだけで、元になったpngファイルには反映されない)
    private void UpdatePixel(Vector2 position)
    {
        var rx = position.x / Screen.width * Tex4Write.width;
        var ry = position.y / Screen.height * Tex4Write.height;

        if (rx < 0 || rx > Tex4Write.width || ry < 0 || ry > Tex4Write.height)
            return;

        var col = copiedTex.GetPixel((int)rx, (int)ry);

        //SetPixelsSquare(copiedTex, (int)rx, (int)ry);
        SetPixelsCircle(copiedTex, (int)rx, (int)ry);

        WriteRawImage.texture = copiedTex;
        //Tex4Write = copiedTex;
        Mat.mainTexture = copiedTex;
    }

    private void SetPixelsSquare(Texture2D tex, int x, int y)
    {
        // Textureにピクセルカラーを設定する
        tex.SetPixels(x, y, brush.Width, brush.Height, brush.Colors);
        // 反映
        copiedTex.Apply();
    }

    private void SetPixelsCircle(Texture2D tex, int x, int y)
    {
        var center = new Vector2(x, y);
        int r = brush.Width;
        for (int iy = -r; iy < r; iy++)
        {
            for (int ix = -r; ix < r; ix++)
            {
                if (ix * ix + iy * iy < r * r)
                    tex.SetPixel(x + ix, y + iy, brush.Color);
            }
        }
        copiedTex.Apply();
    }
}
