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

    private ImageScrollModel imagesModel;

    void Start ()
    {
        var items = viewer.ViewContent.GetComponentsInChildren<CanvasRenderer>().Select(i => i.GetComponent<RectTransform>()).ToArray();
        imagesModel = new ImageScrollModel(items, MAX_VIEW_ITEMS);

        viewer.Initialize(MAX_VIEW_ITEMS, imagesModel.ItemsCount, imagesModel.SpriteList);//, imagesModel.FirstItem.anchoredPosition.y);
        viewer.OnUpdateItemsByScrollDown = OnScrollDown;
        viewer.OnUpdateItemsByScrollUp = OnScrollUp;

        imagesModel.OnUpdateImage = viewer.OnUpdateImage;
    }
	
    private void OnScrollDown(int columnCnt, int currentRow, LinkedList<RectTransform> list)// float anchoredY)
    {
        imagesModel.OnChangeDrawByScrollDown(columnCnt, currentRow, list);// anchoredY);
        Resources.UnloadUnusedAssets();
    }
    private void OnScrollUp(int columnCnt, int currentRow, LinkedList<RectTransform> list)// float anchoredY)
    {
        imagesModel.OnChangeDrawByScrollUp(columnCnt, currentRow, list);// anchoredY);
        Resources.UnloadUnusedAssets();
    }
}
