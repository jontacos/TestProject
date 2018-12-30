using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public int pattern = 0;
	IEnumerator Start () {
        yield return new WaitForSeconds(1f);
        isEaseTest = true;

    }

    bool isEaseTest = false;
    float time = 2f;
    float ela = 0f;
	void Update () {
		if(Input.GetKeyUp(KeyCode.A))
        {
            isEaseTest = true;
            ela = 0;
        }
        {
            //transform.RotateAxisX(45f);
            transform.rotation *= Quaternion.Euler(1f, 1f, 0);
        }

        //UtilTouch.GetTouchPosition();
        //UtilTouch.GetTouchWorldPosition(Camera.main);
        //UtilTouch.GetTouchPosRatio();

        if (ela > time || !isEaseTest) return;
        var val = 0f;
        var start = -8f;
        var y = 0f;
        if(pattern == 0)
            val = Jontacos.Utils.EaseIn(start, 8, ela, time);
        else if(pattern == 1)
            val = Jontacos.Utils.EaseOut(start, 8, ela, time);
        else if(pattern == 2)
            val = Jontacos.Utils.EaseInOut(start, 8, ela, time);
        else
        {
            var pos = Jontacos.Utils.EaseOut(new Vector2(start, -1), new Vector2(8, 8), ela, time);
            transform.SetPositionY(pos.y);
            val = pos.x;
        }
        transform.SetPositionX(val);
        ela += Time.deltaTime;
    }

}
