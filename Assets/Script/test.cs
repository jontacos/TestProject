using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetKeyUp(KeyCode.A))
        {
            //transform.RotateAxisX(45f);
            transform.rotation *= Quaternion.Euler(1f, 1f, 0);
        }

        //UtilTouch.GetTouchPosition();
        //UtilTouch.GetTouchWorldPosition(Camera.main);
        //UtilTouch.GetTouchPosRatio();

    }
}
