using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jontacos;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class BoneController : MonoBehaviour
{
    public IKManager2D IKManager;
    public SpriteRenderer Butterflies;

	void Start ()
    {
        //TextureLoad();
    }

    float elapse = 0f;
	void Update ()
    {

    }

    //private void TextureLoad()
    //{
    //    var tmp = Resources.Load<Texture2D>("Textures/ColoringPages/" + Butterflies.name);

    //    //オブジェクトの名前からフォルダパスを取得
    //    var path = Utils.GetWriteFolderPath(Butterflies.name);
    //    var filePaths = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => File.GetCreationTime(f)).ToArray();

    //    var tex = Utils.LoadTextureByFileIO(filePaths[0], tmp.width, tmp.height);
    //    //var tex = Utils.LoadTextureByFileIO(Application.streamingAssetsPath + "/" + "SavedScreen.png", tmp.width, tmp.height);

    //    var mat = Butterflies.material;
    //    mat.SetTexture("_SourceTex", tex);
    //}
}
