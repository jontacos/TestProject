using Jontacos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveScreenShot
{

    public IEnumerator Save(EdgeTexture EdgeTex)
    {
        yield return new WaitForEndOfFrame();

        var rate = (float)EdgeTex.Texture.width / EdgeTex.Texture.height;
        var r = 1820f / 1920f;
        // 画面全体ではなく表示されている書き込み用キャンバスと同サイズにする
        var height = Screen.height;
        var width = Mathf.RoundToInt(Screen.height * rate);
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.name = EdgeTex.Texture.name;
        tex.ReadPixels(new Rect((Screen.width * r - width) / 2, 0, width, height), 0, 0);
        tex.Apply();

        yield return EdgeTex.StartCoroutine(WriteFile(tex));
        yield return null;
    }

    private IEnumerator WriteFile(Texture2D tex)
    {
        var fileName = "Screenshot" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

        var path = Utils.GetWriteFolderPath(tex.name);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path += fileName;

        var bytes = tex.EncodeToPNG();
#if !UNITY_EDITOR
        //var bytes = tex.GetRawTextureData();
        File.WriteAllBytes(path, bytes);
        yield return new WaitForEndOfFrame();

        while (!File.Exists(path))
            yield return new WaitForEndOfFrame();

        UtilsAndroid.ScanFile(path, null);
#else

        Debug.Log("WriteFile");
        File.WriteAllBytes(path/*"SavedScreen0.png"*/, bytes);
        yield return null;
#endif
    }
}
