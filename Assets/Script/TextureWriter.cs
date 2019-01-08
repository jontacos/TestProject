using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureWriter : MonoBehaviour
{

    public Material Mat;
    public RawImage WriteRawImage;
    public RawImage EdgeTex;

    public GameObject Pointer;
    public GameObject View;

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
        private static Color[] ColorTypes = new Color[] { Color.white, Color.red, Color.green, Color.blue, Color.yellow, Color.gray };

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

    private Page writePage;


    void Start ()
    {
        brush = new Brush();
        Mat = View.GetComponent<MeshRenderer>().material;

        CreatePage();
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

    public void ColorChange(int value)
    {
        brush.UpdateColor(value);
    }

    public void CreatePage()
    {
        writePage = new Page();
        writePage.SetCanvas(WriteRawImage);
    }

    private void UpdatePixel(Vector2 position)
    {
        var rx = position.x / Screen.width * writePage.TexWidth;
        var ry = position.y / Screen.height * writePage.TexHeight;
        
        if (rx < 0 || rx > writePage.TexWidth || ry < 0 || ry > writePage.TexHeight)
            return;

        writePage.WriteCircle((int)rx, (int)ry, brush.Color, brush.Width);
    }
}
