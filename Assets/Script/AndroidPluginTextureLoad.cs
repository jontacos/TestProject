using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPluginTextureLoad : AndroidPluginReciever
{
    public EdgeTexture EdgeTex;

    protected override void Start()
    {
        base.Start();
    }
    public override void OnCallBack(string dataPath)
    {
        Debug.Log("-----OnCallback-----");
        // 画像が選択されたら、取得したパスを元に画像をロードする.
        ImageLoad(dataPath);
    }

    private void ImageLoad(string path)
    {
        Debug.Log("-----ImageLoad-----");
        if (File.Exists(path))
        {
            var ext = Path.GetExtension(path).ToLower();
            Debug.Log("------" + path + "," + ext + "------");
            Texture2D tex;
            if(ext == ".bmp")
            {
                Debug.Log("-----BMPLoad-----");
                tex = Jontacos.BitmapLoader.Load(ext);
            }
            else
            {
                Debug.Log("-----Load-----");
                // バイト配列でファイルを読み込み、Texture2Dとしてセットする.
                byte[] bytReadBinary = File.ReadAllBytes(path);
                var width = Screen.width;
                var height = Screen.height;
                tex = new Texture2D(width, height);
                tex.LoadImage(bytReadBinary);
                tex.filterMode = FilterMode.Trilinear;
                tex.Apply();
            }
            EdgeTex.SetTextureSizeByAspect(tex);
        }
        else
            Debug.LogError("-----File Dont Exist-----");
    }
}
