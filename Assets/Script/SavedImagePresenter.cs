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
        viewer.OnUpdateItemsByScrollDown = OnScrollDown;
        viewer.OnUpdateItemsByScrollUp = OnScrollUp;

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

    private void SetNodeTexture4EdgeTexture(Texture2D tex)
    {
        painter.SetEdgeTexture(tex);
        Close();
    }
	
    private void OnScrollDown(int columnCnt, int currentRow, LinkedList<RectTransform> list)
    {
        imagesModel.OnChangeDrawByScrollDown(columnCnt, currentRow, list);
        Resources.UnloadUnusedAssets();
    }
    private void OnScrollUp(int columnCnt, int currentRow, LinkedList<RectTransform> list)
    {
        imagesModel.OnChangeDrawByScrollUp(columnCnt, currentRow, list);
        Resources.UnloadUnusedAssets();
    }
}
