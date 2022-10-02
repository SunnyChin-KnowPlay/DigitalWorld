using DigitalWorld.Asset;
using DigitalWorld.Events;
using DigitalWorld.UI;
using DreamEngine.UI;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Notices.UI
{
    /// <summary>
    /// 通知面板
    /// </summary>
    public class NoticePanel : PanelControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Notice/NoticePanel.prefab";
        #endregion

        #region Params
        /// <summary>
        /// 通知
        /// </summary>
        private RectTransform noticesRectTransform;

        /// <summary>
        /// 通知缓存池
        /// </summary>
        private readonly Stack<GameObject> noticesStack = new Stack<GameObject>();

        private readonly List<WidgetTitle> runningNotices = new List<WidgetTitle>();
        /// <summary>
        /// 标题对象路径
        /// </summary>
        private const string titleObjectPath = "Assets/Res/UI/Elements/Title/DoubleIconTitle.prefab";
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            noticesRectTransform = GetControlComponent<RectTransform>("Root/Notices");
        }

        private void LateUpdate()
        {
            for (int i = 0; i < this.runningNotices.Count; ++i)
            {
                WidgetTitle title = runningNotices[i];
                if (!title.gameObject.activeSelf)
                {
                    RecycleTitleObject(title);
                    runningNotices.RemoveAt(i);
                    --i;
                }
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // 通知
            Events.EventManager.Instance.RegisterListener(Events.EEventType.Notice_Board, OnNotice);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // 通知
            Events.EventManager.Instance?.UnregisterListener(Events.EEventType.Notice_Board, OnNotice);
        }
        #endregion

        #region Logic
        private void RecycleTitleObject(WidgetTitle notice)
        {
            RectTransform rt = notice.RectTransform;
            rt.SetParent(null);

            GameObject go = notice.gameObject;
            go.SetActive(false);

            this.noticesStack.Push(go);
        }

        private WidgetTitle AllocateTitleObject()
        {
            GameObject go = null;
            if (noticesStack.Count <= 0)
            {
                GameObject obj = AssetManager.LoadAsset<GameObject>(titleObjectPath);
                go = GameObject.Instantiate(obj);
            }
            else
            {
                go = this.noticesStack.Pop();
            }
            if (null != go)
            {
                if (!go.TryGetComponent<WidgetTitle>(out WidgetTitle title))
                {
                    title = go.AddComponent<WidgetTitle>();
                }

                return title;
            }

            return null;
        }

        public void ShowNotice(string message, float duration)
        {
            WidgetTitle title = this.AllocateTitleObject();

            if (null != title)
            {
                title.gameObject.SetActive(true);

                title.SetTitle(message);
                title.DelayHide(duration);

                RectTransform rt = title.RectTransform;
                rt.SetParent(noticesRectTransform, false);
                rt.localScale = Vector3.one;

                this.runningNotices.Add(title);
            }
        }
        #endregion

        #region Listen
        protected virtual void OnNotice(Events.EEventType type, System.EventArgs args)
        {
            if (args is EventArgsNotice notice)
            {
                ShowNotice(notice.message, notice.duration);


            }
        }
        #endregion
    }
}
