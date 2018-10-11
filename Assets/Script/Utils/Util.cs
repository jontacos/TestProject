using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    #region 拡張メソッド
    // position
    public static Transform SetPositionX(this Transform t, float x)
    {
        t.position = new Vector3(x, t.position.y, t.position.z);
        return t;
    }
    public static Transform SetPositionY(this Transform t, float y)
    {
        t.position = new Vector3(t.position.x, y, t.position.z);
        return t;
    }
    public static Transform SetPositionZ(this Transform t, float z)
    {
        t.position = new Vector3(t.position.x, t.position.y, z);
        return t;
    }
    public static Transform SetLocalPositionX(this Transform t, float x)
    {
        t.localPosition = new Vector3(x, t.position.y, t.position.z);
        return t;
    }
    public static Transform SetLocalPositionY(this Transform t, float y)
    {
        t.localPosition = new Vector3(t.position.x, y, t.position.z);
        return t;
    }
    public static Transform SetLocalPositionZ(this Transform t, float z)
    {
        t.localPosition = new Vector3(t.position.x, t.position.y, z);
        return t;
    }
    public static Transform SetPositionXY(this Transform t, float x, float y)
    {
        t.position = new Vector3(x, y, t.position.z);
        return t;
    }
    public static Transform SetPositionXY(this Transform t, Vector2 xy)
    {
        t.position = new Vector3(xy.x, xy.y, t.position.z);
        return t;
    }
    public static Transform SetPositionYZ(this Transform t, float y, float z)
    {
        t.position = new Vector3(t.position.x, y, z);
        return t;
    }
    public static Transform SetPositionXZ(this Transform t, float x, float z)
    {
        t.position = new Vector3(x, t.position.y, z);
        return t;
    }
    public static Transform SetLocalPositionXY(this Transform t, float x, float y)
    {
        t.localPosition = new Vector3(x, y, t.position.z);
        return t;
    }
    public static Transform SetLocalPositionXY(this Transform t, Vector2 xy)
    {
        t.localPosition = new Vector3(xy.x, xy.y, t.position.z);
        return t;
    }
    public static Transform SetLocalPositionYZ(this Transform t, float y, float z)
    {
        t.localPosition = new Vector3(t.position.x, y, z);
        return t;
    }
    public static Transform SetLocalPositionXZ(this Transform t, float x, float z)
    {
        t.localPosition = new Vector3(x, t.position.y, z);
        return t;
    }

    // rotate
    public static Transform SetRotateX(this Transform t, float x)
    {
        t.localRotation = Quaternion.Euler(new Vector3(x, t.rotation.eulerAngles.y, t.rotation.eulerAngles.z));
        return t;
    }
    public static Transform SetRotateY(this Transform t, float y)
    {
        t.localRotation = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, y, t.rotation.eulerAngles.z));
        return t;
    }
    public static Transform SetRotateZ(this Transform t, float z)
    {
        t.localRotation = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, z));
        return t;
    }

    public static Transform RotateAxisX(this Transform t, float x)
    {
        t.localRotation *= Quaternion.Euler(new Vector3(x, 0, 0));
        return t;
    }
    public static Transform RotateAxisY(this Transform t, float y)
    {
        t.localRotation *= Quaternion.Euler(new Vector3(0, y, 0));
        return t;
    }
    public static Transform RotateAxisZ(this Transform t, float z)
    {
        t.localRotation *= Quaternion.Euler(new Vector3(0, 0, z));
        return t;
    }


    // scale
    public static Transform SetScaleX(this Transform t, float x)
    {
        t.localScale = new Vector3(x, t.localScale.y, t.localScale.z);
        return t;
    }
    public static Transform SetScaleY(this Transform t, float y)
    {
        t.localScale = new Vector3(t.localScale.x, y, t.localScale.z);
        return t;
    }
    public static Transform SetScaleZ(this Transform t, float z)
    {
        t.localScale = new Vector3(t.localScale.x, t.localScale.y, z);
        return t;
    }
    public static Transform SetScaleXY(this Transform t, float x, float y)
    {
        t.localScale = new Vector3(x, y, t.localScale.z);
        return t;
    }
    public static Transform SetScaleXY(this Transform t, Vector2 xy)
    {
        t.localScale = new Vector3(xy.x, xy.y, t.localScale.z);
        return t;
    }
    #endregion
}
