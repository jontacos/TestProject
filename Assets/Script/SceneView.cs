using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneView : MonoBehaviour
{
    [SerializeField]
    private UniqueMenuButton SceneChangeButton;

    void Start()
    {
        
    }

    public void OnPushSceneChange(Action onPush)
    {
        SceneChangeButton.OnPush = onPush;
    }
}
