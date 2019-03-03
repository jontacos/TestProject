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
    /// 現在画面上で一番上に表示させている列番号
    /// </summary>
    private int currentItemRowNo = 0;

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

    /// <summary>
    /// 表示アイテム用リスト
    /// </summary>
    public LinkedList<RectTransform> ItemList { get; private set; }
    //private LinkedList<RectTransform> itemList = new LinkedList<RectTransform>();

    /// <summary>
    /// アイテム表示列数
    /// </summary>
    [Range(1, 100)]
    public int ColumnCount = 3;

    /// <summary>
    /// 1つのアイテムの高さ(空きスペース含め)
    /// </summary>
    public int ItemsHeight = 110;

    ///// <summary>
    ///// スクロールによるアンカー位置の調整用
    ///// </summary>
    //public float ChangedAnchoredY { get; private set; }

    /// <summary>
    /// 下スクロールによって画像更新が行われる際のコールバック
    /// </summary>
    public Action<int, int, LinkedList<RectTransform>/*float*/> OnUpdateItemsByScrollDown;
    /// <summary>
    /// 上スクロールによって画像更新が行われる際のコールバック
    /// </summary>
    public Action<int, int, LinkedList<RectTransform>/*float*/> OnUpdateItemsByScrollUp;

    protected virtual void Awake()
    {
        scroll = GetComponent<ScrollRect>();
        ItemList = new LinkedList<RectTransform>();
        var items = viewContent.GetComponentsInChildren<CanvasRenderer>().Select(i => i.GetComponent<RectTransform>()).ToArray();
        foreach (var item in items)
            ItemList.AddLast(item);
    }

    protected virtual void Start()
    {
        for (int i = 0; i < ItemList.Count; ++i)
        {
            var item = ItemList.ElementAt(i);
            item.GetComponent<Image>().color = Color.white;
            //if (i > ItemsCount - 1)
            item.gameObject.SetActive(false);
        }
    }

    protected void Update()
    {
        LoopItemsOnUndraw();
    }

    public void Initialize(int viewObjCnt, int itemsCount, List<Sprite> list)//, float y)
    {
        maxViewObjectCount = viewObjCnt;
        startedTopItemAnchoredY = ItemList.First.Value.anchoredPosition.y;//y;

        scrollHeight = Mathf.Max(1, itemsCount / ColumnCount) * ItemsHeight;
        viewContent.offsetMin = new Vector2(0, -scrollHeight);

        for (int i = 0; i < list.Count; ++i)
        {
            var image = ItemList.ElementAt(i).GetComponent<Image>();
            image.sprite = list[i];
            image.gameObject.SetActive(true);
        }
    }

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
            for (int i = 0; i < ColumnCount; ++i)
            {
                var topItem = ItemList.First.Value;
                topItem.gameObject.SetActive(false);
                ItemList.RemoveFirst();
                ItemList.AddLast(topItem);
                topItem.anchoredPosition = new Vector2(topItem.anchoredPosition.x, //anchoredY);
                    startedTopItemAnchoredY - scrollDiff - ItemsHeight * (maxViewObjectCount / ColumnCount - 1));
            }

            //ChangedAnchoredY = startedTopItemAnchoredY - scrollDiff - ItemsHeight * (maxViewObjectCount / ColumnCount - 1);
            if (OnUpdateItemsByScrollDown != null)
                OnUpdateItemsByScrollDown(ColumnCount, currentItemRowNo, ItemList/*ChangedAnchoredY*/);
        }
        // 上スクロール時
        else if(anchoredPosY - scrollDiff < 0 && scrollDiff > 1)
        {
            scrollDiff -= ItemsHeight;
            --currentItemRowNo;

            for (int i = 0; i < ColumnCount; ++i)
            {
                var bottomItem = ItemList.Last.Value;
                bottomItem.gameObject.SetActive(false);
                ItemList.RemoveLast();
                ItemList.AddFirst(bottomItem);
                bottomItem.anchoredPosition = new Vector2(bottomItem.anchoredPosition.x, //anchoredY);
                    startedTopItemAnchoredY - scrollDiff);
            }
            //ChangedAnchoredY = startedTopItemAnchoredY - scrollDiff;
            if(OnUpdateItemsByScrollUp != null)
                OnUpdateItemsByScrollUp(ColumnCount, currentItemRowNo, ItemList/*ChangedAnchoredY*/);
        }
    }

    public void OnUpdateImage(int idx, Sprite sprite)
    {
        var item = ItemList.ElementAt(idx).GetComponent<Image>();
        item.sprite = sprite;
        item.gameObject.SetActive(true);
    }
}

