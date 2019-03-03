using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewer : MonoBehaviour
{
    ///// <summary>
    ///// 表示に利用するオブジェクト数
    ///// </summary>
    //protected static readonly int MAX_VIEW_ITEMS = 12;

    [SerializeField]
    private RectTransform viewContent;
    public RectTransform ViewContent { get { return viewContent; } }

    private ScrollRect scroll;

    private int maxViewObjectCount;

    /// <summary>
    /// スクロール補正用
    /// </summary>
    private float scrollDiff = 0f;

    /// <summary>
    /// スクロールの高さ
    /// </summary>
    private float scrollHeight = 0f;

    /// <summary>
    /// 初期状態での一番上アイテムのアンカーY値
    /// </summary>
    private float startedTopItemAnchoredY = 0;

    ///// <summary>
    ///// 表示アイテム用リスト
    ///// </summary>
    //protected LinkedList<RectTransform> itemList = new LinkedList<RectTransform>();

    ///// <summary>
    ///// 表示アイテム数
    ///// </summary>
    //protected int itemsCount = 0;

    /// <summary>
    /// 下スクロールによって画像更新が行われる際のコールバック
    /// </summary>
    public Action<int, int, float> OnUpdateItemsByScrollDown;
    /// <summary>
    /// 上スクロールによって画像更新が行われる際のコールバック
    /// </summary>
    public Action<int, int, float> OnUpdateItemsByScrollUp;

    /// <summary>
    /// アイテム列が上下に移動した際に更新させる処理
    /// arg1:画面表示上の最上段列番号, arg2:下スクロールならtrue
    /// </summary>
    protected Action<int, bool> OnUpdateViewItems;

    /// <summary>
    /// 現在画面上で一番上に表示させている列番号
    /// </summary>
    private int currentItemRowNo = 0;

    /// <summary>
    /// アイテム表示列数
    /// </summary>
    [Range(1,100)]
    public int ColumnCount = 3;

    /// <summary>
    /// 1つのアイテムの高さ(空きスペース含め)
    /// </summary>
    public int ItemsHeight = 110;

    protected virtual void Awake()
    {
        scroll = GetComponent<ScrollRect>();
        var items = viewContent.GetComponentsInChildren<CanvasRenderer>().Select(i => i.GetComponent<RectTransform>()).ToArray();
        //foreach(var item in items)
        //    itemList.AddLast(item);
    }

    protected virtual void Start()
    {
        //SetScrollerHeight();
        //for (int i = 0; i < itemList.Count; ++i)
        //{
        //    var item = itemList.ElementAt(i);
        //    item.GetComponent<Image>().color = Color.white;
        //    if (i > itemsCount - 1)
        //        item.gameObject.SetActive(false);
        //}
        //startedTopItemAnchoredY = itemList.First.Value.anchoredPosition.y;
    }

    protected void Update()
    {
        LoopItemsOnUndraw();
    }

    public void Initialize(int viewObjCnt, int itemsCount, float y)
    {
        maxViewObjectCount = viewObjCnt;
        startedTopItemAnchoredY = y;

        scrollHeight = Mathf.Max(1, itemsCount / ColumnCount) * ItemsHeight;
        viewContent.offsetMin = new Vector2(0, -scrollHeight);
    }

    ///// <summary>
    ///// ScrollViewのスクロール幅の指定
    ///// </summary>
    //public void SetScrollerHeight(int itemsCount)
    //{
    //    scrollHeight = Mathf.Max(1, itemsCount / ColumnCount) * ItemsHeight;
    //    viewContent.offsetMin = new Vector2(0, -scrollHeight);
    //}

    public float ChangedAnchoredY { get; private set; }
    /// <summary>
    /// アイテムの描画が見えなくなったものからループさせる
    /// </summary>
    private void LoopItemsOnUndraw()
    {
        var anchoredPosY = viewContent.anchoredPosition.y;

        // 下スクロール時
        if (anchoredPosY - scrollDiff > ItemsHeight && scrollHeight - anchoredPosY > ItemsHeight * 3)
        {
            scrollDiff += ItemsHeight;
            ++currentItemRowNo;

            //for (int i = 0; i < ColumnCount; ++i)
            //{
            //    var topItem = itemList.First.Value;
            //    itemList.RemoveFirst();
            //    itemList.AddLast(topItem);
            //    topItem.anchoredPosition = new Vector2(topItem.anchoredPosition.x,
            //        startedTopItemAnchoredY - scrollDiff - ItemsHeight * (MAX_VIEW_ITEMS / ColumnCount - 1));
            //}

            ChangedAnchoredY = startedTopItemAnchoredY - scrollDiff - ItemsHeight * (maxViewObjectCount / ColumnCount - 1);
            if (OnUpdateItemsByScrollDown != null)
                OnUpdateItemsByScrollDown(ColumnCount, currentItemRowNo, ChangedAnchoredY);

            //if (OnUpdateViewItems != null)
            //    OnUpdateViewItems(currentItemRowNo, true);
        }
        // 上スクロール時
        else if(anchoredPosY - scrollDiff < 0 && scrollDiff > 1)
        {
            scrollDiff -= ItemsHeight;
            --currentItemRowNo;

            //for (int i = 0; i < ColumnCount; ++i)
            //{
            //    var bottomItem = itemList.Last.Value;
            //    itemList.RemoveLast();
            //    itemList.AddFirst(bottomItem);
            //    bottomItem.anchoredPosition = new Vector2(bottomItem.anchoredPosition.x,
            //        startedTopItemAnchoredY - scrollDiff);
            //}

            ChangedAnchoredY = startedTopItemAnchoredY - scrollDiff;
            if(OnUpdateItemsByScrollUp != null)
                OnUpdateItemsByScrollUp(ColumnCount, currentItemRowNo, ChangedAnchoredY);

            //if (OnUpdateViewItems != null)
            //    OnUpdateViewItems(currentItemRowNo, false);
        }
    }
}

