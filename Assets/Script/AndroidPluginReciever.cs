using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPluginReciever : MonoBehaviour
{
    private static readonly string PACKAGE_NAME_NATIVE_PLUGIN = "androidcam.jontacos.unity.mylibrary.CameraRoll";
    private RawImage texObj;

    void Start()
    {
        texObj = gameObject.GetComponent<RawImage>();
    }
    public void OnCallBack(string dataPath)
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
            // バイト配列でファイルを読み込み、Texture2Dとしてセットする.
            byte[] bytReadBinary = File.ReadAllBytes(path);
            var width = Screen.width;
            var height = Screen.height;
            var tex = new Texture2D(width, height);
            tex.LoadImage(bytReadBinary);
            texObj.texture = tex;
        }
        else
            Debug.LogError("-----File Dont Exist-----");
    }
}
