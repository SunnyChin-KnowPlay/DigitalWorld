using DreamEngine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DreamEngine.UI
{
    public class WidgetListView : Widget
    {
        #region Define
        public enum ESelectType
        {
            Single,
            Multiple
        }

        public enum EFlowType
        {
            Horizontal,
            Vertical,
        }

        private class ItemInfo
        {
            public WidgetListViewItem item;
            public bool isSelected;
        }
        #endregion

        #region Fields
        [SerializeField] private ESelectType selectType;//选择类型
        [SerializeField] private EFlowType flowType;//滑动类型
        [SerializeField] private int constraintCount;//固定的行数或列数，若为0则根据窗口大小自动计算行列数
        [SerializeField] private Vector2 itemSpace;//item间距
        #endregion

        #region Params
        /// <summary>
        /// 数据队列
        /// </summary>
        private List<IWidgetListViewItemData> datas = null;
        public bool IsVirtual => true;
        public GameObject ItemPrefab { get; set; }

        private Action<int, bool> onItemValueChanged;//item是否选中的状态发生改变时调用
        private Action<WidgetListViewItem> onItemClicked;//item被点击时调用
        public Action onSelectedItemCleared;

        private int rowCount, columnCount;//上下滑动时，列数固定。左右滑动时，行数固定
        private Vector2 initialSize;//listview窗口大小，用于计算行列数
        private Vector2 itemSize;//item大小

        private new RectTransform rectTransform;
        private ScrollRect scrollRect;
        private GameObjectPool pool;

        private List<ItemInfo> itemInfoList;//用做虚列表
        private int itemRealCount;//用做虚列表，真实的item数量
        private int startIndex, endIndex;//用做虚列表，视图中左上角item的下标，以及结束的下标
        private bool isBoundsDirty;

        public int ItemCount
        {
            get => IsVirtual ? itemRealCount : itemList.Count;
            set
            {
                ResetPosition();
                ClearAllSelectedItem();
                if (IsVirtual)
                {
                    itemRealCount = value;
                    SetSize();
                    int oldCount = itemInfoList.Count;
                    if (itemRealCount > oldCount)
                    {
                        for (int i = oldCount; i < itemRealCount; i++)
                        {
                            ItemInfo info = new ItemInfo();
                            itemInfoList.Add(info);
                        }
                    }
                    else
                    {
                        for (int i = itemRealCount; i < oldCount; i++)
                        {
                            if (itemInfoList[i].item != null)
                            {
                                RemoveItem(itemInfoList[i].item);
                                itemInfoList[i].item = null;
                            }
                        }
                    }
                }
                else
                {
                    int oldCount = itemList.Count;
                    if (value > oldCount)
                    {
                        for (int i = oldCount; i < value; i++)
                            AddItem();
                    }
                    else
                        RemoveItem(value, oldCount - 1);
                }
                Refresh();
            }
        }

        private List<WidgetListViewItem> itemList;
        private List<WidgetListViewItem> selectedItemList;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
            rectTransform.pivot = Vector2.up;
            rectTransform.anchorMax = Vector2.up;
            rectTransform.anchorMin = Vector2.up;

            itemList = new List<WidgetListViewItem>();
            selectedItemList = new List<WidgetListViewItem>();
            itemInfoList = new List<ItemInfo>();
            scrollRect = rectTransform.GetComponentInParent<ScrollRect>();
            if (scrollRect == null)
                Debug.LogError("ListView can not find ScrollRect");
            scrollRect.onValueChanged.AddListener(OnScroll);
        }

        private void Update()
        {
            if (isBoundsDirty)
                UpdateBounds();
        }

        private void OnDestroy()
        {
            pool?.Clear();

            itemList.Clear();
            selectedItemList.Clear();
            itemInfoList.Clear();
        }
        #endregion

        #region Logic
        public void Init(GameObject prefab, List<IWidgetListViewItemData> datas, Action<int, bool> valueChanged, Action<WidgetListViewItem> clicked)
        {
            this.datas = datas;
            ItemPrefab = prefab;
            pool = new GameObjectPool(rectTransform, ItemPrefab);
            onItemValueChanged = valueChanged;
            onItemClicked = clicked;

            GetLayoutAttribute();
        }

        public void Refresh()
        {
            RenderVirtualItem(true);
        }

        public void ClearAllSelectedItem()
        {
            if (IsVirtual)
            {
                for (int i = startIndex; i < ItemCount; i++)
                    itemInfoList[i].isSelected = false;
            }
            else
            {
                for (int i = 0, count = selectedItemList.Count; i < count; i++)
                {
                    selectedItemList[i].IsSelected = false;
                }
                selectedItemList.Clear();
            }
            onSelectedItemCleared?.Invoke();
        }

        public WidgetListViewItem AddItem()
        {
            if (ItemPrefab == null) return null;

            GameObject go = pool.Allocate();
            go.transform.SetParent(rectTransform);
            go.transform.localScale = Vector3.one;
            if (!go.TryGetComponent<WidgetListViewItem>(out WidgetListViewItem item))
            {
                item = go.AddComponent<WidgetListViewItem>();
            }
            itemList.Add(item);
            item.Init(OnValueClicked);
            go.SetActive(true);
            SetBoundsDirty();
            return item;
        }

        public void RemoveItem(WidgetListViewItem item)
        {
            if (item.IsSelected)
            {
                item.IsSelected = false;
                OnValueChanged(item);
            }
            pool.Recycle(item.gameObject);
            itemList.Remove(item);
            SetBoundsDirty();
        }

        public void RemoveItem(int index)
        {
            if (index < 0 || index >= itemList.Count) return;
            RemoveItem(itemList[index]);
        }

        public void RemoveItem(int beginIndex, int endIndex)
        {
            if (beginIndex > endIndex) return;
            for (int i = beginIndex; i <= endIndex; i++)
                RemoveItem(beginIndex);
        }

        public void RemoveAllItem()
        {
            RemoveItem(0, ItemCount - 1);
        }

        private void SetBoundsDirty()
        {
            isBoundsDirty = true;
        }

        private void UpdateBounds()
        {
            if (IsVirtual)
                return;
            SetSize();
            for (int i = 0, count = ItemCount; i < count; i++)
                itemList[i].transform.localPosition = CalculatePosition(i);

            isBoundsDirty = false;
        }

        private void SetSize()
        {
            rectTransform.sizeDelta = flowType == EFlowType.Horizontal ? new Vector2(GetContentLength(), initialSize.y) : new Vector2(initialSize.x, GetContentLength());
        }

        private void ResetPosition()
        {
            startIndex = 0;
            endIndex = 0;
            rectTransform.localPosition = Vector3.zero;
        }

        //根据item下标计算所在位置
        private Vector2 CalculatePosition(int index)
        {
            int row, column;
            if (flowType == EFlowType.Horizontal)
            {
                row = index % rowCount;
                column = index / rowCount;
            }
            else
            {
                row = index / columnCount;
                column = index % columnCount;
            }

            float x = column * (itemSize.x + itemSpace.x) + itemSize.x / 2;
            float y = row * (itemSize.y + itemSpace.y) + itemSize.y / 2;

            return new Vector2(x, -y);
        }

        private float GetContentLength()
        {
            if (ItemCount == 0) return 0;
            //计算所有item需要的长度
            if (flowType == EFlowType.Horizontal)
            {
                int columnCount = Mathf.CeilToInt((float)ItemCount / rowCount);
                return itemSize.x * columnCount + itemSpace.x * (columnCount - 1);
            }
            else
            {
                int rowCount = Mathf.CeilToInt((float)ItemCount / columnCount);
                return itemSize.y * rowCount + itemSpace.y * (rowCount - 1);
            }
        }

        private void OnScroll(Vector2 position)
        {
            if (IsVirtual)
                RenderVirtualItem(false);
        }

        private void RenderVirtualItem(bool isForceRender)
        {
            ScrollAndRender(isForceRender);
        }

        //isForceRender:是否强制更新所有item
        private void ScrollAndRender(bool isForceRender)
        {
            int oldStartIndex = startIndex, oldEndIndex = endIndex;
            if (flowType == EFlowType.Horizontal)
            {
                float currentX = rectTransform.localPosition.x;// <0
                startIndex = GetCurrentIndex(currentX);
                float endX = currentX - initialSize.x - itemSize.x - itemSpace.x;
                endIndex = GetCurrentIndex(endX);
            }
            else
            {
                float currentY = rectTransform.localPosition.y;// >0
                                                               //上下滑动，根据listview的y值计算当前视图中第一个item的下标
                startIndex = GetCurrentIndex(currentY);
                //根据视图高度，item高度，间距的y，计算出结束行的下标
                float endY = currentY + initialSize.y + itemSize.y + itemSpace.y;
                endIndex = GetCurrentIndex(endY);
            }

            if (oldStartIndex == startIndex && oldEndIndex == endIndex)
                return;

            //渲染当前视图内需要显示的item
            for (int i = startIndex; i < ItemCount && i < endIndex; i++)
            {
                bool needRender = false;//是否需要刷新item ui
                ItemInfo info = itemInfoList[i];

                if (info.item == null)
                {
                    int j, jEnd;
                    if (oldStartIndex < startIndex || oldEndIndex < endIndex)
                    {
                        //说明是往下或者往右滚动，即要从前面找复用的Item
                        j = 0;
                        jEnd = startIndex;
                    }
                    else
                    {
                        j = endIndex;
                        jEnd = ItemCount;
                    }
                    for (; j < jEnd; j++)
                    {
                        if (itemInfoList[j].item != null)
                        {
                            info.item = itemInfoList[j].item;
                            itemInfoList[j].item = null;
                            needRender = true;
                            break;
                        }
                    }
                }

                //前后找不到的话，添加新的item
                if (info.item == null)
                {
                    info.item = AddItem();
                    needRender = true;
                }

                //更新位置，是否选中状态，以及数据
                if (isForceRender || needRender)
                {
                    info.item.transform.localPosition = CalculatePosition(i);
                    info.item.IsSelected = info.isSelected;

                    this.RefreshItem(i, info.item);
                }
            }
        }

        //根据listview的位置，计算该位置的行或列的第一个item的下标
        private int GetCurrentIndex(float position)
        {
            if (flowType == EFlowType.Horizontal)
            {
                position = -position;
                if (position < itemSize.x) return 0;
                position -= itemSize.x;
                return (Mathf.FloorToInt(position / (itemSize.x + itemSpace.x)) + 1) * rowCount;
            }
            else
            {
                if (position < itemSize.y) return 0;
                position -= itemSize.y;
                return (Mathf.FloorToInt(position / (itemSize.y + itemSpace.y)) + 1) * columnCount;
            }
        }

        private void GetLayoutAttribute()
        {
            itemSize = ItemPrefab.GetComponent<RectTransform>().rect.size;
            initialSize = rectTransform.parent.GetComponent<RectTransform>().rect.size;//Viewport Size

            //计算行或列
            if (flowType == EFlowType.Horizontal)
            {
                rowCount = constraintCount;
                if (rowCount <= 0)
                    rowCount = Mathf.FloorToInt((initialSize.y + itemSpace.y) / (itemSize.y + itemSpace.y));
                if (rowCount == 0)
                    rowCount = 1;
            }
            else
            {
                columnCount = constraintCount;
                if (columnCount <= 0)
                    columnCount = Mathf.FloorToInt((initialSize.x + itemSpace.x) / (itemSize.x + itemSpace.x));
                if (columnCount == 0)
                    columnCount = 1;
            }
        }

        private void OnValueChanged(WidgetListViewItem item)
        {
            if (item.IsSelected)
            {
                if (selectType == ESelectType.Single)
                {
                    for (int i = 0; i < ItemCount; i++)
                    {
                        //找到对应项，设置为选中
                        if (itemInfoList[i].item == item)
                        {
                            itemInfoList[i].isSelected = true;
                            onItemValueChanged?.Invoke(i, true);
                            continue;
                        }

                        //取消之前的选中状态
                        if (itemInfoList[i].isSelected)
                        {
                            itemInfoList[i].isSelected = false;
                            if (itemInfoList[i].item != null)
                                itemInfoList[i].item.IsSelected = false;
                            onItemValueChanged?.Invoke(i, false);
                        }
                    }
                }
                else
                {
                    //找到对应项，设置为选中
                    for (int i = startIndex; i < ItemCount; i++)
                    {
                        if (itemInfoList[i].item == item)
                        {
                            itemInfoList[i].isSelected = true;
                            onItemValueChanged?.Invoke(i, true);
                            break;
                        }
                    }
                }
            }
            else
            {
                //找到对应项，设置为未选中
                for (int i = startIndex; i < ItemCount; i++)
                {
                    if (itemInfoList[i].item == item)
                    {
                        itemInfoList[i].isSelected = false;
                        onItemValueChanged?.Invoke(i, false);
                        break;
                    }
                }
            }
        }

        private void OnValueClicked(WidgetListViewItem item)
        {
            if (this.selectType == ESelectType.Single)
            {
                bool isSelected = item.IsSelected;
                this.ClearAllSelectedItem();
                item.IsSelected = !isSelected;
            }
            else
            {
                item.IsSelected = !item.IsSelected;
            }

            onItemClicked?.Invoke(item);
        }

        /// <summary>
        /// 刷新条目
        /// </summary>
        /// <param name="item"></param>
        private void RefreshItem(int index, WidgetListViewItem item)
        {
            if (index >= 0 && index < datas.Count)
            {
                IWidgetListViewItemData data = this.datas[index];
                item.Setup(data);
            }

        }
        #endregion

    }

}

