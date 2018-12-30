using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPluginAccessor
{
    void CallStatic(string method, params object[] args);
}

public class AndroidPluginAccessor : IPluginAccessor
{
    public static readonly string SHOW_TOAST = "ShowToast";
    public static readonly string OPEN_CAM_ROLL = "Open";

    private static readonly string PACKAGE_NAME_NATIVE_PLUGIN = "androidcam.jontacos.unity.mylibrary.CameraRoll";

    private static AndroidJavaClass javaClass;


    public void CallStatic(string method, params object[] args)
    {
        using (javaClass = new AndroidJavaClass(PACKAGE_NAME_NATIVE_PLUGIN))
        {
            javaClass.CallStatic(method, args);
        }
    }
}
