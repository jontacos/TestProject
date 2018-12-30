using UnityEngine;

namespace Jontacos
{
    public enum TouchInfo
    {
        None = 99,

        // 以下は UnityEngine.TouchPhase の値に対応
        Began = 0,
        Moved,
        Stationary,
        Ended,
        Canceled,
    }


    public static class UtilTouch
    {
        private static Vector3 TouchPosition = Vector3.zero;

        public static TouchInfo GetTouch()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
            if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
            if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
#else
        if (Input.touchCount > 0)
        {
            return (TouchInfo)Input.GetTouch(0).phase;
        }
#endif
            return TouchInfo.None;
        }

        /// <summary>
        /// タッチ(クリック)座標取得。
        /// タッチしていない場合、xyzすべて-10000。
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetTouchPosition()
        {
            if (GetTouch() == TouchInfo.None)
                return -Vector3.one * 10000;

            TouchPosition = GetPosition();

            Debug.Log("タッチ座標：" + TouchPosition);
            return TouchPosition;
        }

        /// <summary>
        /// タッチ座標のワールド変換。
        /// タッチしていない場合、xyzすべて-1。
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Vector3 GetTouchWorldPosition(Camera cam)
        {
            if (GetTouch() == TouchInfo.None)
                return -Vector3.one;

            TouchPosition = cam.ScreenToWorldPoint(GetPosition());
            Debug.Log("タッチワールド座標：" + TouchPosition);
            return TouchPosition;
        }

        /// <summary>
        /// タッチ座標の画面比率。
        /// タッチしていない場合、xyzすべて-1。
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetTouchPosRatio()
        {
            if (GetTouch() == TouchInfo.None)
                return -Vector3.one;

            TouchPosition = GetPosition();

            var ratio = new Vector3(TouchPosition.x / Screen.width, TouchPosition.y / Screen.height);
            Debug.Log("タッチ座標比率：" + ratio);
            return ratio;
        }

        private static Vector3 GetPosition()
        {
#if UNITY_EDITOR
            return Input.mousePosition;
#else
        var touch = Input.GetTouch(0);
        return touch.position;
#endif
        }
    }
}