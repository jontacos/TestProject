using System.Collections;
using System.Collections.Generic;
using Jontacos;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class BoneController : MonoBehaviour
{
    public IKManager2D IKManager;
    public SpriteRenderer SPRenderer;

	void Start ()
    {
        TextureLoad();
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

    private void TextureLoad()
    {
        var tmp = Resources.Load<Texture2D>("Textures/ColoringPages/in05");
        //var tex = new Texture2D(tmp.width, tmp.height);
        //var filePath = Application.streamingAssetsPath + "/" + "SavedScreen.png";
        //byte[] b = File.ReadAllBytes(filePath);
        //tex.LoadImage(b, false);
        var tex = Utils.LoadTextureByFileIO(Application.streamingAssetsPath + "/" + "SavedScreen.png", tmp.width, tmp.height);

        var mat = SPRenderer.material;
        mat.SetTexture("_SourceTex", tex);
    }
}
