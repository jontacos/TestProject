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

    private string[] fileNames;

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
        //CheckSaveImageNums();
        //LoadImagesOnInit();
        LoadColoringPagesFromResourcesOnInit();
    }

    /// <summary>
    /// お絵描き表示用画像のロード
    /// </summary>
    private void LoadColoringPagesFromResourcesOnInit()
    {
        //var path = Application.dataPath + "/Resources/Textures/ColoringPages";
        //Debug.Log(path);
        var sprites = Resources.LoadAll("Textures/ColoringPages", typeof(Sprite));
        ItemsCount = sprites.Length;

        fileNames = new string[sprites.Length];
        for (var i = 0; i < ItemsCount; ++i)
        {
            spriteList.Add((Sprite)sprites[i]);
            fileNames[i] = sprites[i].name;
        }
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
        fileNames = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => File.GetCreationTime(f)).ToArray();
        ItemsCount = fileNames.Length;
    }

    /// <summary>
    /// 最初にフォルダにある画像枚数分(ViewObjの最大数まで)をロード
    /// </summary>
    private void LoadImagesOnInit()
    {
        var cnt = Mathf.Min(fileNames.Length, maxViewObjectCount);
        for (int i = 0; i < cnt; ++i)
        {
            var path = fileNames[i];
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
    /// <param name="columnCnt">表示する列数</param>
    /// <param name="currentRow">現在表示している一番上の行番号</param>
    /// <param name="isScrollDown">スクロールダウンでの変化かどうか</param>
    /// <param name="list">ノード登録リスト</param>
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
            if (i >= fileNames.Length)
                break;
            var path = fileNames[i];
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
    /// <summary>
    /// スクロールでの画像更新
    /// </summary>
    /// <param name="columnCnt">表示する列数</param>
    /// <param name="currentRow">現在表示している一番上の行番号</param>
    /// <param name="isScrollDown">スクロールダウンでの変化かどうか</param>
    /// <param name="list">ノード登録リスト</param>
    public void OnChangeDrawByScroll(int columnCnt, int currentRow, bool isScrollDown, LinkedList<RectTransform> list)
    {
        LoadNextRowImages(columnCnt, currentRow, isScrollDown, list);
    }
}
