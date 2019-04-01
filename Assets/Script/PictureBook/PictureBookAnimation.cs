using Jontacos;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class PictureBookAnimation : MonoBehaviour
{
    private float elapse = 0f;
    public float Value;

    protected virtual void Start ()
    {
        TextureLoad();
    }
	
	protected virtual void Update ()
    {
    }

    /// <summary>
    /// お絵描きした画像をロード
    /// </summary>
    protected void TextureLoad()
    {
        var tmp = Resources.Load<Texture2D>("Textures/ColoringPages/" + name);

        //オブジェクトの名前からフォルダパスを取得
        var path = Utils.GetWriteFolderPath(name);

        if (!Directory.Exists(path))
            return;

        // ランダムに選択
        var filePaths = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => File.GetCreationTime(f)).ToArray();
        if(filePaths.Length == 0)
            return;

        int num = Random.Range(0, filePaths.Length);
        var tex = Utils.LoadTextureByFileIO(filePaths[num], tmp.width, tmp.height);
        //var tex = Utils.LoadTextureByFileIO(Application.streamingAssetsPath + "/" + "SavedScreen.png", tmp.width, tmp.height);

        SetTexture(tex);
    }

    private void SetTexture(Texture2D tex)
    {
        var mat = GetComponent<SpriteRenderer>().material;
        mat.SetTexture("_SourceTex", tex);
    }
}
