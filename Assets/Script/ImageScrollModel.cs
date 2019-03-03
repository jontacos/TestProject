using Jontacos;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageScrollModel
{
    /// <summary>
    /// 表示アイテム数
    /// </summary>
    public int ItemsCount { get; private set; }

    /// <summary>
    /// 表示オブジェクトの最大数
    /// </summary>
    private int maxViewObjectCount;

    private string[] filePaths;

    /// <summary>
    /// 表示アイテム用リスト
    /// </summary>
    private LinkedList<RectTransform> itemList = new LinkedList<RectTransform>();
    public RectTransform FirstItem { get { return itemList.First.Value; } }

    public ImageScrollModel(RectTransform[] items, int viewObjCnt)
    {
        foreach (var item in items)
            itemList.AddLast(item);
        maxViewObjectCount = viewObjCnt;
        Initialize();
    }

    private void Initialize()
    {
        CheckSaveImageNums();
        for (int i = 0; i < itemList.Count; ++i)
        {
            var item = itemList.ElementAt(i);
            item.GetComponent<Image>().color = Color.white;
            if (i > ItemsCount - 1)
                item.gameObject.SetActive(false);
        }
        LoadImagesOnInit();
    }

    /// <summary>
    /// 保存画像の枚数チェック
    /// </summary>
    private void CheckSaveImageNums()
    {
        filePaths = Directory.GetFiles(Application.streamingAssetsPath, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => File.GetCreationTime(f)).ToArray();
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
            var tex = Utils.LoadTextureByFileIO(path, 100, 100);
            var item = itemList.ElementAt(i).GetComponent<Image>();
            item.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            item.sprite.name = Path.GetFileName(path);
        }
    }
    /// <summary>
    /// スクロールで更新されるアイテム画像のロード
    /// </summary>
    private void LoadNextRowImages(int columnCnt,int currentRow, bool isScrollDown)
    {
        var loadRow = currentRow + maxViewObjectCount / columnCnt;
        var idx = (loadRow - 1) * columnCnt;
        var diff = columnCnt;
        if (!isScrollDown)
        {
            idx = currentRow * columnCnt;
            diff = 0;
        }

        for (int i = idx; i < idx + columnCnt; ++i)
        {
            if (i >= filePaths.Length)
                break;
            var path = filePaths[i];
            var tex = Utils.LoadTextureByFileIO(path, 100, 100);
            var item = itemList.ElementAt(i - diff).GetComponent<Image>();
            item.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            item.sprite.name = Path.GetFileName(path);
        }
    }

    public void OnChangeDrawByScrollDown(int columnCnt, int currentRow, float anchoredY)
    {
        for (int i = 0; i < columnCnt; ++i)
        {
            var topItem = itemList.First.Value;
            itemList.RemoveFirst();
            itemList.AddLast(topItem);
            topItem.anchoredPosition = new Vector2(topItem.anchoredPosition.x, anchoredY);
                //startedTopItemAnchoredY - scrollDiff - ItemsHeight * (MAX_VIEW_ITEMS / ColumnCount - 1));
        }
        LoadNextRowImages(columnCnt, currentRow, true);
    }
    public void OnChangeDrawScrollUp(int columnCnt, int currentRow, float anchoredY)
    {

        for (int i = 0; i < columnCnt; ++i)
        {
            var bottomItem = itemList.Last.Value;
            itemList.RemoveLast();
            itemList.AddFirst(bottomItem);
            bottomItem.anchoredPosition = new Vector2(bottomItem.anchoredPosition.x, anchoredY);
                //startedTopItemAnchoredY - scrollDiff);
        }
        LoadNextRowImages(columnCnt, currentRow, false);
    }
}
