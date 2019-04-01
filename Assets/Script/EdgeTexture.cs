using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EdgeTexture : MonoBehaviour
{
    private static readonly float MAX_EDGE_TEX_X = 1820;
    private static readonly float MAX_EDGE_TEX_Y = 1080;

    private RawImage edgeTex;
    public Texture Texture { get { return edgeTex.texture; } set { edgeTex.texture = value; } }
    public RectTransform RectTransform { get { return edgeTex.rectTransform; } }

    void Awake ()
    {
        edgeTex = GetComponent<RawImage>();
    }
	
    public void LoadTexture()
    {
#if !UNITY_EDITOR
        var accessor = new AndroidPluginAccessor();
        accessor.CallStatic(AndroidPluginAccessor.OPEN_CAM_ROLL);
#else
        //var t = Resources.Load<Texture2D>("Textures/1yamv0g1");
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

        //var tex = Jontacos.BitmapLoader.Load("E:/Program/TestProject/Assets/Resources/Textures/c0kqmtaw.bmp");
        var tex = Resources.Load<Texture2D>("Textures/ColoringPages/an13");
        SetTextureByAspect(tex);
#endif
    }

    public void SetTextureByAspect(Texture2D tex)
    {
        Debug.Log("W:" + tex.width + ", H;" + tex.height);
        var rate = 0f;
        var w = 0f;
        var h = 0f;
        if (tex.width < MAX_EDGE_TEX_X && tex.width < tex.height)
        {
            rate = MAX_EDGE_TEX_Y / tex.height;
            if (tex.width > tex.height)
            {

            }
            w = MAX_EDGE_TEX_X - tex.width * rate;
        }
        else
        {
            rate = MAX_EDGE_TEX_X / tex.width;
            h = MAX_EDGE_TEX_Y - tex.height * rate;
        }
        Debug.Log(rate);

        var left = w * 0.5f;
        var right = -w * 0.5f;
        var bottom = h * 0.5f;
        var top = -h * 0.5f;
        edgeTex.rectTransform.offsetMax = new Vector2(right, top);
        edgeTex.rectTransform.offsetMin = new Vector2(left, bottom);
        edgeTex.texture = tex;
    }
}
