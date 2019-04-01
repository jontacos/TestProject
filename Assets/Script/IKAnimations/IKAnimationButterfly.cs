using Jontacos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKAnimationButterfly : IKAnimationBase
{
    private Transform[] rightWing;
    private Transform[] leftWing;

    protected override void Start ()
    {
        base.Start();

        rightWing = new Transform[2];
        leftWing = new Transform[2];
        rightWing[0] = animationParts[0];
        rightWing[1] = animationParts[3];
        leftWing[0] = animationParts[1];
        leftWing[1] = animationParts[2];

    }
	
	protected override void Update ()
    {

        foreach (var part in rightWing)
        {
            part.SetRotateY(Rotate);
        }
        foreach (var part in leftWing)
        {
            part.SetRotateY(-Rotate);
        }
    }


}
