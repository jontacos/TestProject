using Jontacos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageScrollViewer : ScrollViewer
{

    //private string[] filePaths;

    protected override void Awake()
    {
        base.Awake();
        //CheckSaveImageNums();
    }
    protected override void Start()
    {
        base.Start();
        //LoadImages();
        //OnUpdateViewItems += LoadNextRowImages;
    }

    ///// <summary>
    ///// 保存画像の枚数チェック
    ///// </summary>
    //private void CheckSaveImageNums()
    //{
    //    filePaths = Directory.GetFiles(Application.streamingAssetsPath, "*.png", SearchOption.TopDirectoryOnly).OrderBy(f => File.GetCreationTime(f)).ToArray();
    //    itemsCount = filePaths.Length;
    //}

    //private void LoadImages()
    //{
    //    var cnt = Mathf.Min(filePaths.Length, MAX_VIEW_ITEMS);
    //    for (int i = 0; i < cnt; ++i)
    //    {
    //        var path = filePaths[i];
    //        var tex = Utils.LoadTextureByFileIO(path, 100, 100);
    //        var item = itemList.ElementAt(i).GetComponent<Image>();
    //        item.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    //        item.sprite.name = Path.GetFileName(path);
    //    }
    //}
    ///// <summary>
    ///// スクロールで更新されるアイテム画像のロード
    ///// </summary>
    //private void LoadNextRowImages(int currentRow, bool isScrollDown)
    //{
    //    var loadRow = currentRow + MAX_VIEW_ITEMS / ColumnCount;
    //    var idx = (loadRow - 1) * ColumnCount;
    //    var diff = ColumnCount;
    //    if (!isScrollDown)
    //    {
    //        idx = currentRow * ColumnCount;
    //        diff = 0;
    //    }

    //    for (int i = idx; i < idx + ColumnCount; ++i)
    //    {
    //        if (i >= filePaths.Length)
    //            break;
    //        var path = filePaths[i];
    //        var tex = Utils.LoadTextureByFileIO(path, 100, 100);
    //        var item = itemList.ElementAt(i - diff).GetComponent<Image>();
    //        item.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    //        item.sprite.name = Path.GetFileName(path);
    //    }
    //}
}