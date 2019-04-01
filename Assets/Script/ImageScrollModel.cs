using Jontacos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageScrollModel
{
    /// <summary>
    /// 表示物の合計数
    /// </summary>
    public int ItemsCount { get; private set; }

    /// <summary>
    /// 表示オブジェクトの最大数
    /// </summary>
    private int maxViewObjectCount;

    private string[] filePaths;

    /// <summary>
    /// ロード画像キャッシュ用リスト
    /// </summary>
    private List<Sprite> spriteList = new List<Sprite>();
    public List<Sprite> SpriteList { get { return spriteList; } }

    /// <summary>
    /// 切り替える画像取得時にView側の画像更新処理呼び出し用
    /// </summary>
    public Action<int, Sprite> OnUpdateImage;

    public ImageScrollModel(RectTransform[] items, int viewObjCnt)
    {
        maxViewObjectCount = viewObjCnt;
        Initialize();
    }

    private void Initialize()
    {
        CheckSaveImageNums();
        LoadImagesOnInit();
    }

    /// <summary>
    /// 保存画像の枚数チェック
    /// </summary>
    private void CheckSaveImageNums()
    {
#if UNITY_EDITOR
        var path = Application.streamingAssetsPath;
#else
        var path = Application.persistentDataPath + "/ScreenShots/";
#endif
        Debug.Log("-----------" + path + "~~~~~~~~~~~~~~");
        filePaths = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => File.GetCreationTime(f)).ToArray();
        ItemsCount = filePaths.Length;
    }


    /// <summary>
    /// 最初にフォルダにある画像枚数分(ViewObjの最大数まで)をロード
    /// </summary>
    private void LoadImagesOnInit()
    {
        var cnt = Mathf.Min(filePaths.Length, maxViewObjectCount);
        for (int i = 0; i < cnt; ++i)
        {
            var path = filePaths[i];
            //var tex = Utils.LoadTextureByFileIO(path, 100, 100);
            var tex = Utils.LoadTextureByWebRequest(path, 100, 100);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            sprite.name = Path.GetFileName(path);
            spriteList.Add(sprite);
        }
    }
    /// <summary>
    /// スクロールで更新されるアイテム画像のロード
    /// </summary>
    private void LoadNextRowImages(int columnCnt,int currentRow, bool isScrollDown, LinkedList<RectTransform> list)
    {
        int idx = 0;
        int diff = currentRow * columnCnt;
        if (isScrollDown)
        {
            var loadRow = currentRow + maxViewObjectCount / columnCnt;
            idx = (loadRow - 1) * columnCnt;
        }
        else
        {
            idx = currentRow * columnCnt;
        }

        for (int i = idx; i < idx + columnCnt; ++i)
        {
            if (i >= filePaths.Length)
                break;
            var path = filePaths[i];
            var name = Path.GetFileName(path);
            Sprite sprite = spriteList.FirstOrDefault(s => s.name == name);
            if (sprite == null)
            {
                //var tex = Utils.LoadTextureByFileIO(path, 100, 100);
                var tex = Utils.LoadTextureByWebRequest(path, 100, 100);
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                sprite.name = name;
                spriteList.Add(sprite);
            }

            if (OnUpdateImage != null)
                OnUpdateImage(i - diff, sprite);
        }
    }

    public void OnChangeDrawByScrollDown(int columnCnt, int currentRow, LinkedList<RectTransform> list)
    {
        LoadNextRowImages(columnCnt, currentRow, true, list);
    }
    public void OnChangeDrawByScrollUp(int columnCnt, int currentRow, LinkedList<RectTransform> list)
    {
        LoadNextRowImages(columnCnt, currentRow, false, list);
    }
}
