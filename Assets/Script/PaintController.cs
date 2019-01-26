using System;
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
    public RawImage ScreenShot;

    public GameObject Pointer;
    public GameObject View;
    public GameObject ColorPullet;

    private Page writePage;
    private bool isOpendColorPullet = true;
    private bool isMovingPullet = false;
    private bool isUnpaintable = false;

    private Vector2 prePos;

    void Start ()
    {
        brush = new Brush();
        Mat = View.GetComponent<MeshRenderer>().material;
        prePos = Vector2.zero;

        CreatePage();
        LoadTexture();
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
        else
            prePos = Vector2.zero;
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
        //// XYでテクスチャの端点を決められ、WHでテクスチャサイズを変更できる
        ////EdgeTex.uvRect = new Rect(EdgeTex.uvRect.x, EdgeTex.uvRect.y, EdgeTex.texture.width, EdgeTex.texture.height);
        ////EdgeTex.SetNativeSize();
        //byte[] bytReadBinary = File.ReadAllBytes("E:/Program/TestProject/Assets/Resources/Textures/1yamv0g1");
        //var width = Screen.width;
        //var height = Screen.height;
        //var tex = new Texture2D(width, height);
        //tex.LoadImage(bytReadBinary);
        //tex.filterMode = FilterMode.Trilinear;
        //tex.Apply();

        var tex = Jontacos.BitmapLoader.Load("E:/Program/TestProject/Assets/Resources/Textures/c0kqmtaw.bmp");
        EdgeTex.texture = tex;
#endif
    }

    private void UpdatePixel(Vector2 position)
    {
        var rx = position.x / (Screen.width * 1820 / 1920) * writePage.TexWidth;
        var ry = position.y / Screen.height * writePage.TexHeight;
        
        if (rx < 0 || rx > writePage.TexWidth || ry < 0 || ry > writePage.TexHeight)
            return;


        if (prePos.sqrMagnitude > 1)
        {
            var pos = new Vector2(rx, ry);
            var nor = (pos - prePos).normalized;
            var len = Mathf.Abs((int)Vector2.Distance(pos, prePos));
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

    public void OnSaveButton()
    {
        StartCoroutine(SaveScreenShot());
    }
    private IEnumerator SaveScreenShot()
    {
        ColorPullet.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();
        Debug.Log(Screen.width);
        var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        StartCoroutine(WriteFile(tex));
        yield return null;

        ColorPullet.gameObject.SetActive(true);
    }


    private IEnumerator WriteFile(Texture2D tex)
    {
        Debug.Log("WriteFile");
        var fileName = "Screenshot" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

#if !UNITY_EDITOR
        //保存パス取得
        using (AndroidJavaClass jcEnv = new AndroidJavaClass("android.os.Environment"))
        using (AndroidJavaObject obj = jcEnv.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jcEnv.GetStatic<string>("DIRECTORY_PICTURES"/*"DIRECTORY_DCIM"*/ )))
        {
            var output = obj.Call<string>("toString");
            output += "/ScreenShots/" + fileName;
            //var bytes = tex.GetRawTextureData();
            var bytes = tex.EncodeToPNG();
            File.WriteAllBytes(output, bytes);
            yield return new WaitForEndOfFrame();

            while (!File.Exists(output))
                yield return new WaitForEndOfFrame();

            Debug.Log("MediaDirWriteFileOK:" + output);

            ScanFile(output, null);
        }
#else
        ScreenShot.texture = tex;
        ScreenShot.gameObject.SetActive(true);
        yield return null;
#endif
    }

    //インデックス情報にファイル名を登録する
    //これをしないとPC から内部ストレージを参照した時にファイルが見えない
    void ScanFile(string path, string mimeType)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
        using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
        using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
        using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
        {
            Debug.Log("------scanFile:" + path + "--------");
            var mimeTypes = (mimeType != null) ? new string[] { mimeType } : null;
            jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { path }, mimeTypes, null);
        }
        Handheld.StopActivityIndicator();
    }
}
