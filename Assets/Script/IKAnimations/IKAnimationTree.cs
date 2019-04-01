using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKAnimationTree : IKAnimationBase
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        foreach (var part in animationParts)
        {
            part.SetRotateZ(90f + Rotate);
        }
    }
}
