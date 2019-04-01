using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private enum SceneName
    {
        BaseScene,
        ColoringPage,
        PictureBook,
    }

    public SceneView SceneView;

    private int preSceneIndex;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //起動時にお絵描きシーンに遷移させておく
        LoadScene((int)SceneName.ColoringPage);
    }

    void Start()
    {
        SceneView.OnPushSceneChange(ChangeScene);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void LoadScene(int idx)
    {
        SceneManager.LoadSceneAsync(idx, LoadSceneMode.Additive);
    }

    private void ChangeScene()
    {
        var scene = SceneManager.GetActiveScene();
        preSceneIndex = scene.buildIndex;
        // 現在使用するシーンは1と2のみなので交互にロードされるようにしておく
        int idx = (preSceneIndex % 2) + 1;
        LoadScene(idx);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        if (preSceneIndex != (int)SceneName.BaseScene)
            SceneManager.UnloadSceneAsync(preSceneIndex);
    }
}
