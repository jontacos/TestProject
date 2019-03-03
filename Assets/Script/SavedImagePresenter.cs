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
    private ImageScrollViewer viewer;

    private ImageScrollModel imagesModel;

    void Start ()
    {
        var items = viewer.ViewContent.GetComponentsInChildren<CanvasRenderer>().Select(i => i.GetComponent<RectTransform>()).ToArray();
        imagesModel = new ImageScrollModel(items, MAX_VIEW_ITEMS);

        viewer.Initialize(MAX_VIEW_ITEMS, imagesModel.ItemsCount, imagesModel.FirstItem.anchoredPosition.y);
        viewer.OnUpdateItemsByScrollDown = OnScrollDown;
        viewer.OnUpdateItemsByScrollUp = OnScrollUp;
    }
	
    private void OnScrollDown(int columnCnt, int currentRow, float anchoredY)
    {
        imagesModel.OnChangeDrawByScrollDown(columnCnt, currentRow, anchoredY);
        Resources.UnloadUnusedAssets();
    }
    private void OnScrollUp(int columnCnt, int currentRow, float anchoredY)
    {
        imagesModel.OnChangeDrawScrollUp(columnCnt, currentRow, anchoredY);
        Resources.UnloadUnusedAssets();
    }
}
