using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PaintController : MonoBehaviour
{
    private static readonly float PULLET_DOWN_Y = -130;

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

    public Material Mat;
    public RawImage WriteRawImage;
    public RawImage EdgeTex;

    public GameObject Pointer;
    public GameObject View;
    public GameObject ColorPullet;

    private Page writePage;
    private bool isOpendColorPullet = true;
    private bool isMovingPullet = false;
    private bool isUnpaintable = false;

    void Start ()
    {
        brush = new Brush();
        Mat = View.GetComponent<MeshRenderer>().material;

        CreatePage(); LoadTexture();
    }
	
	void Update ()
    {
        //if (isUnpaintable)
        //    return;
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

    public void LoadTexture()
    {
#if !UNITY_EDITOR
        var accessor = new AndroidPluginAccessor();
        accessor.CallStatic(AndroidPluginAccessor.OPEN_CAM_ROLL);
#else
        var t = Resources.Load<Texture2D>("Textures/1yamv0g1");
        // XYでテクスチャの端点を決められ、WHでテクスチャサイズを変更できる
        //EdgeTex.uvRect = new Rect(EdgeTex.uvRect.x, EdgeTex.uvRect.y, EdgeTex.texture.width, EdgeTex.texture.height);
        //EdgeTex.SetNativeSize();
        byte[] bytReadBinary = File.ReadAllBytes("E:/Program/TestProject/Assets/Resources/Textures/1yamv0g1");
        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height);
        tex.LoadImage(bytReadBinary);
        tex.filterMode = FilterMode.Trilinear;
        tex.Apply();
        EdgeTex.texture = tex;
#endif
    }

    private void UpdatePixel(Vector2 position)
    {
        var rx = position.x / Screen.width * writePage.TexWidth;
        var ry = position.y / Screen.height * writePage.TexHeight;
        
        if (rx < 0 || rx > writePage.TexWidth || ry < 0 || ry > writePage.TexHeight)
            return;

        writePage.WriteCircle((int)rx, (int)ry, brush.Color, brush.Width);
    }

    public void ExpandBrush()
    {
        brush.Width += 1;
    }
    public void ShrinkBrush()
    {
        if(brush.Width > 0)
            brush.Width -= 1;
    }
    public void SelectColorPullet()
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
        var t = 0f;
        while(t < time)
        {
            var y = Jontacos.Utils.EaseOut(PULLET_DOWN_Y, 0, t,time);
            ColorPullet.transform.SetLocalPositionY(y);
            t += Time.deltaTime;
            yield return null;
        }
        isMovingPullet = false;
        isOpendColorPullet = false;
        isUnpaintable = true;
    }
    private IEnumerator CloseColorPullet(float time)
    {
        var t = 0f;
        while (t < time)
        {
            var y = Jontacos.Utils.EaseOut(0, PULLET_DOWN_Y, t, time);
            ColorPullet.transform.SetLocalPositionY(y);
            t += Time.deltaTime;
            yield return null;
        }
        isMovingPullet = false;
        isOpendColorPullet = true;
        isUnpaintable = false;
    }
}
