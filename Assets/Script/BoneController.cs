using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class BoneController : MonoBehaviour {

    public IKManager2D IKManager;

	void Start ()
    {
        

    }

    float elapse = 0f;
	void Update ()
    {
        elapse += Time.deltaTime;
        var solvers = IKManager.solvers;
        foreach(var s in solvers)
        {
            var child = s.transform.GetChild(0);
            child.transform.SetRotateY(Mathf.Sin(elapse) * 30f);
        }

    }
}
