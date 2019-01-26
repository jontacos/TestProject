using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidPluginReciever : MonoBehaviour
{
    private static readonly string PACKAGE_NAME_NATIVE_PLUGIN = "androidcam.jontacos.unity.mylibrary.CameraRoll";

    protected virtual void Start()
    {
    }
    public virtual void OnCallBack(string dataPath)
    {
        Debug.Log("-----OnCallback-----");
    }
}
