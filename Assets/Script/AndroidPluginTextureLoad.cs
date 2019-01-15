using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPluginTextureLoad : AndroidPluginReciever
{
    public RawImage texObj;

    protected void Start()
    {
        base.Start();
        texObj = gameObject.GetComponent<RawImage>();
    }
    public override void OnCallBack(string dataPath)
    {
        Debug.Log("-----OnCallback-----");
        // 画像が選択されたら、取得したパスを元に画像をロードする.
        ImageLoad(dataPath);
    }

    private void ImageLoad(string path)
    {
        if (File.Exists(path))
        {
            Debug.Log("-----ImageLoad-----");
            // バイト配列でファイルを読み込み、Texture2Dとしてセットする.
            byte[] bytReadBinary = File.ReadAllBytes(path);
            var width = Screen.width;
            var height = Screen.height;
            var tex = new Texture2D(width, height);
            tex.LoadImage(bytReadBinary);
            tex.filterMode = FilterMode.Trilinear;
            tex.Apply();
            texObj.texture = tex;
        }
        else
            Debug.LogError("-----File Dont Exist-----");
    }
}
