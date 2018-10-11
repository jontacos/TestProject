using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidCamera : MonoBehaviour
{
    //[SerializeField]
    private int width = 1920;
    //[SerializeField]
    private int height = 1080;
    [SerializeField]
    private RawImage displayUI;
    [SerializeField]
    private Image CaptureIamge;

    private bool isCaptured = false;
    private WebCamTexture webCamTex;
    private Texture2D screenShotTex;
    private Quaternion baseRotation;

	IEnumerator Start ()
    {
        width = Screen.width;
        height = Screen.height;
        screenShotTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        
        baseRotation = displayUI.transform.rotation;

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

        var webCamDevice = WebCamTexture.devices[0];
        webCamTex = new WebCamTexture(webCamDevice.name, width, height);

        displayUI.texture = webCamTex;
        webCamTex.Play();
    }
	
	void Update ()
    {
        if (webCamTex == null)
            return;

        // 端末の回転に合わせて表示テクスチャも回転させる
        displayUI.transform.rotation = baseRotation * Quaternion.AngleAxis(webCamTex.videoRotationAngle, Vector3.forward);

        if(isCaptured)
        {
            StartCoroutine(FadeOutCapturedImage());

        }
    }

    //public void OnPlay()
    //{
    //    if (webCamTex == null)
    //        return;
    //    webCamTex.Play();
    //}
    //public void OnStop()
    //{
    //    if (webCamTex == null)
    //        return;
    //    webCamTex.Stop();
    //}
    public void OnCapture()
    {
        if (webCamTex == null || isCaptured)
            return;
        screenShotTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShotTex.Apply();
        CaptureIamge.sprite = Sprite.Create(screenShotTex, new Rect(0, 0, width, height), Vector2.zero);
        CaptureIamge.gameObject.SetActive(true);
        isCaptured = true;
    }

    private IEnumerator FadeOutCapturedImage()
    {
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => UtilTouch.GetTouch() != TouchInfo.None);

        var time = 1f;
        var t = 0f;
        var col = Color.white;
        while (t < time)
        {
            col.a = 1f - t / time;
            CaptureIamge.color = col;
            t += Time.deltaTime;
            yield return null;
        }
        CaptureIamge.gameObject.SetActive(false);
        CaptureIamge.color = Color.white;
        isCaptured = false;
    }
}
