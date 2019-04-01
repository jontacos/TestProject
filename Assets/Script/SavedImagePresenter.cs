using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavedImagePresenter : MonoBehaviour
{
    /// <summary>
    /// 表示に利用するオブジェクト数
    /// </summary>
    private static readonly int MAX_VIEW_ITEMS = 12;

    [SerializeField]
    private ScrollViewer viewer;

    [SerializeField]
    private PaintController painter;

    private ImageScrollModel imagesModel;

    public bool IsOpend { get { return gameObject.activeSelf; } }

    private void Start()
    {
        viewer.SetNodeButtonAction(SetNodeTexture4EdgeTexture);
    }

    public void Initialize()
    {
        var items = viewer.ViewContent.GetComponentsInChildren<CanvasRenderer>().Select(i => i.GetComponent<RectTransform>()).ToArray();
        imagesModel = new ImageScrollModel(items, MAX_VIEW_ITEMS);

        viewer.Initialize(MAX_VIEW_ITEMS, imagesModel.ItemsCount, imagesModel.SpriteList);
        viewer.OnUpdateItemsByScroll = OnScroll;

        imagesModel.OnUpdateImage = viewer.OnUpdateImage;
    }
    public void Open()
    {
        painter.SetUnPaintable(true);
        viewer.ResetScrollerPosition();
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
        painter.SetUnPaintable(false);
    }

    /// <summary>
    /// スクロールで選択されたテクスチャを表示用オブジェクトにセット
    /// </summary>
    /// <param name="tex"></param>
    private void SetNodeTexture4EdgeTexture(Texture2D tex)
    {
        painter.SetEdgeTexture(tex);
        Close();
    }

    private void OnScroll(int columnCnt, int currentRow, bool isScrollDown, LinkedList<RectTransform> list)
    {
        imagesModel.OnChangeDrawByScroll(columnCnt, currentRow, isScrollDown, list);
        Resources.UnloadUnusedAssets();
    }
}
