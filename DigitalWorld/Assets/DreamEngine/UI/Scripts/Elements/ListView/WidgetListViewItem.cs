using System;
using UnityEngine;
using UnityEngine.UI;

namespace DreamEngine.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class WidgetListViewItem : Widget
    {
        /// <summary>
        /// 数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : IWidgetListViewItemData
        {
            return (T)data;
        }
        private IWidgetListViewItemData data;

        public int Id { get; private set; }

        private Action<WidgetListViewItem> onClicked;//适用于只在Item被单击时做操作的情况
        private Button button;

        /// <summary>
        /// 是否被选择了
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnRefresh();
                }
            }
        }
        private bool isSelected;

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            Id = GetInstanceID();
            isSelected = false;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClicked);

            rectTransform.anchorMin = Vector2.up;
            rectTransform.anchorMax = Vector2.up;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        #endregion

        #region Logic
        public void Init(Action<WidgetListViewItem> onClicked)
        {
            this.onClicked = onClicked;
        }

        public void Setup(IWidgetListViewItemData data)
        {
            this.data = data;

            this.OnRefresh();
        }

        protected abstract void OnRefresh();

        void OnClicked()
        {
            onClicked?.Invoke(this);
        }


        #endregion
    }
}
