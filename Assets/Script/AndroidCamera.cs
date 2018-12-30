using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AndroidCamera : MonoBehaviour
{
    //[SerializeField]
    private int width = 1920;
    //[SerializeField]
    private int height = 1080;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RawImage displayUI;
    [SerializeField]
    private Image captureIamge;
    [SerializeField]
    private GameObject captureButton;
    [SerializeField]
    private Button CapturedButton;
    [SerializeField]
    private Material BlackAndWhite;

    private bool isCaptured = false;
    private WebCamTexture webCamTex;
    private Texture2D screenShotTex;
    private Quaternion baseRotation;

    private string path = "";
    private string fileName = "";

    public SpriteRenderer renderer;
    IEnumerator Start ()
    {
        width = Screen.width;
        height = Screen.height;
        canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(width, height);
        Debug.Log("W:" + width + ", H:" + height);

        cam = Camera.main;
        path = Application.persistentDataPath + "/";
        screenShotTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        captureIamge.rectTransform.sizeDelta = new Vector2(width, height);
        captureIamge.transform.SetLocalPositionXY(0, 0);
        baseRotation = displayUI.transform.rotation;
        CapturedButton.enabled = false;

        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("カメラのデバイスがない");
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if(!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("カメラの許可がない");
            yield break;
        }

        //var webCamDevice = WebCamTexture.devices[0];
        //webCamTex = new WebCamTexture(webCamDevice.name, width, height);
        //displayUI.texture = webCamTex;
        //webCamTex.Play();
        ////FL_Start();
        ///

        var currentRT = RenderTexture.active;
        var renderTexture = new RenderTexture(displayUI.texture.width, displayUI.texture.height, 32);
        // mainTexture のピクセル情報を renderTexture にコピー
        Graphics.Blit(displayUI.texture, renderTexture);
        // renderTexture のピクセル情報を元に texture2D のピクセル情報を作成
        RenderTexture.active = renderTexture;
        screenShotTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenShotTex.Apply();

        var cols = screenShotTex.GetPixels();

        RenderTexture.active = currentRT;


        // 書き換え用テクスチャ用配列の作成
        Color[] change_pixels = new Color[cols.Length];
        for (int i = 0; i < cols.Length; i++)
        {
            Color pixel = cols[i];

            // 書き換え用テクスチャのピクセル色を指定
            Color change_pixel = new Color(1f, pixel.g, pixel.b, pixel.a);
            change_pixels.SetValue(change_pixel, i);
        }
        // 書き換え用テクスチャの生成
        Texture2D change_texture = new Texture2D(screenShotTex.width, screenShotTex.height, TextureFormat.RGBA32, false);
        change_texture.filterMode = FilterMode.Point;
        change_texture.SetPixels(change_pixels);
        change_texture.Apply();
        // テクスチャを貼り替える
        GetComponent<Renderer>().material.mainTexture = change_texture;


        var accessor = new AndroidPluginAccessor();
        accessor.CallStatic(AndroidPluginAccessor.OPEN_CAM_ROLL);
    }
	
	void Update ()
    {
#if !UNITY_EDITOR
        if (webCamTex == null)
            return;
        // 端末の回転に合わせて表示テクスチャも回転させる
        displayUI.transform.rotation = baseRotation * Quaternion.AngleAxis(webCamTex.videoRotationAngle, Vector3.forward);
#endif
        if (Jontacos.UtilTouch.GetTouch() == Jontacos.TouchInfo.Began || Jontacos.UtilTouch.GetTouch() == Jontacos.TouchInfo.Moved)
        {
            var pos = Jontacos.UtilTouch.GetTouchPosition();
            BlackAndWhite.SetFloat("_PosX", pos.x / width);
            BlackAndWhite.SetFloat("_PosY", pos.y / height);
        }
    }


    void OnDestroy()
    {
        //FL_Stop();
    }

    public void OnCapture()
#if !UNITY_EDITOR
    {
        //if (isCaptured)
        //    return;

        //StartCoroutine(Capture());
    }
#else
    {
        if (isCaptured)
            return;
        captureIamge.rectTransform.SetScaleXY(1f, 1f);
        captureIamge.transform.SetLocalPositionXY(Vector2.zero);
        captureIamge.gameObject.SetActive(true);
        captureButton.gameObject.SetActive(false);
        isCaptured = true;
        StartCoroutine(MoveAndDecreaseSizeCapturedImage());
}
#endif

    private IEnumerator Capture()
    {
        captureButton.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        screenShotTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShotTex.Apply();
        captureIamge.color = Color.white;
        captureIamge.rectTransform.SetScaleXY(1f, 1f);
        captureIamge.transform.SetLocalPositionXY(Vector2.zero);
        captureIamge.sprite = Sprite.Create(screenShotTex, new Rect(0, 0, width, height), Vector2.zero);
        captureIamge.gameObject.SetActive(true);
        isCaptured = true;
        StartCoroutine(MoveAndDecreaseSizeCapturedImage());
        StartCoroutine(WriteFile(screenShotTex));
    }

    public void CapturedImageButton()
    {
        Debug.Log("A");
        CapturedButton.enabled = false;
        captureIamge.material = BlackAndWhite;
        captureIamge.rectTransform.SetScaleXY(1f, 1f);
        captureIamge.rectTransform.SetLocalPositionXY(Vector2.zero);
        if (webCamTex == null)
            return;
        webCamTex.Stop();
    }

    private IEnumerator MoveAndDecreaseSizeCapturedImage()
    {
        yield return new WaitForSeconds(0.5f);

        var time = 0.5f;
        var t = 0f;
        var size = 1f;
        var pos = Vector2.zero;
        while (t < time)
        {
            size = Jontacos.Utils.EaseOut(1f, 0.25f, t, time);
            pos = Jontacos.Utils.EaseOut(Vector2.zero, new Vector2(-width * 0.5f + width * 0.25f * 0.5f, -height * 0.5f + height * 0.25f * 0.5f), t, time);
            //pos = RectTransformUtility.WorldToScreenPoint(cam, pos/*new Vector2(-Screen.width,-Screen.height)*/);
            captureIamge.rectTransform.SetScaleXY(size, size);
            captureIamge.rectTransform.SetLocalPositionXY(pos);
            Debug.Log(pos);
            t += Time.deltaTime;
            yield return null;
        }
        isCaptured = false;
        captureButton.gameObject.SetActive(true);
        CapturedButton.enabled = true;
    }


    private IEnumerator WriteFile(Texture2D tex)
    {
        Debug.Log("WriteFile");
        fileName = "Screenshot" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        

#if UNITY_ANDROID
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
#endif
    }

    //インデックス情報にファイル名を登録する
    //これをしないとPC から内部ストレージを参照した時にファイルが見えない
    static void ScanFile(string path, string mimeType)
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

    //private bool Active;
    //private AndroidJavaObject camera1;
    //void FL_Start()
    //{
    //    AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
    //    WebCamDevice[] devices = WebCamTexture.devices;

    //    int camID = 0;
    //    camera1 = cameraClass.CallStatic<AndroidJavaObject>("open", camID);

    //    if (camera1 != null)
    //    {
    //        AndroidJavaObject cameraParameters = camera1.Call<AndroidJavaObject>("getParameters");
    //        cameraParameters.Call("setFlashMode", "torch");
    //        camera1.Call("setParameters", cameraParameters);
    //        camera1.Call("startPreview");
    //        Active = true;
    //    }
    //    else
    //    {
    //        Debug.LogError("[CameraParametersAndroid] Camera not available");
    //    }

    //}
    //void FL_Stop()
    //{

    //    if (camera1 != null)
    //    {
    //        camera1.Call("stopPreview");
    //        camera1.Call("release");
    //        Active = false;
    //    }
    //    else
    //    {
    //        Debug.LogError("[CameraParametersAndroid] Camera not available");
    //    }

    //}


}

