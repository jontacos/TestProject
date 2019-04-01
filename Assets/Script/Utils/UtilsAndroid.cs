using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsAndroid
{
    /// <summary>
    /// パブリックフォルダパス取得
    /// </summary>
    /// <returns></returns>
    public static string GetExternalStoragePublicDirectory()
    {
        using (var jcEnv = new AndroidJavaClass("android.os.Environment"))
        using (var obj = jcEnv.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jcEnv.GetStatic<string>("DIRECTORY_PICTURES")))
        {
            var path = obj.Call<string>("toString");
            return path;
        }
    }
    /// <summary>
    /// アプリフォルダ取得
    /// </summary>
    /// <returns></returns>
    public static string GetExternalStorageFileDirectory()
    {
        using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var obj = activity.Call<AndroidJavaObject>("getExternalFilesDir", null))
        {
            return obj.Call<string>("getAbsolutePath");
        }
    }
    /// <summary>
    /// インデックス情報にファイル名を登録する
    /// これをしないとPC から内部ストレージを参照した時にファイルが見えない
    /// </summary>
    /// <param name="path"></param>
    /// <param name="mimeType"></param>
    public static void ScanFile(string path, string mimeType)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
        using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
        //using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
        //using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
        {
            Debug.Log("------scanFile:" + path + "--------");
            var mimeTypes = (mimeType != null) ? new string[] { mimeType } : null;
            jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { path }, mimeTypes, null);
        }
        Handheld.StopActivityIndicator();
    }

}
