using DreamEngine.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using DigitalWorld.Inputs;

namespace DigitalWorld.Events
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public sealed class EventManager : Singleton<EventManager>
    {
        #region Delegates
        public delegate void OnProcessEventHandle(EEventType eventType, System.EventArgs args);
        #endregion

        #region Params
        /// <summary>
        /// 处理者队列
        /// </summary>
        private readonly Dictionary<EEventType, List<OnProcessEventHandle>> processors = new Dictionary<EEventType, List<OnProcessEventHandle>>();
        private readonly List<OnProcessEventHandle> boardcastingHandles = new List<OnProcessEventHandle>();
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (Input.GetKeyUp(InputManager.Instance.GetKeyCode(EventCode.Escape)))
            {
                this.ApplyEscape();
            }
        }
        #endregion

        #region Logic
        public void Clear()
        {
            processors.Clear();
        }

        private List<OnProcessEventHandle> GetHandles(EEventType type)
        {
            if (!this.processors.TryGetValue(type, out List<OnProcessEventHandle> handles))
            {
                handles = new List<OnProcessEventHandle>();
                this.processors.Add(type, handles);
            }
            return handles;
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handle"></param>
        /// <param name="mode">如果是replace，则会把之前注册监听的同一个delegate的所有监听句柄全部移除</param>
        public void RegisterListener(EEventType eventType, OnProcessEventHandle handle, EHandleAddMode mode = EHandleAddMode.Force)
        {
            List<OnProcessEventHandle> handles = GetHandles(eventType);

            if (mode == EHandleAddMode.Replace)
            {
                UnregisterListener(eventType, handle, EHandleRemoveMode.All);
            }

            handles.Add(handle);
        }

        /// <summary>
        /// 注销监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handle">句柄</param>
        /// <param name="mode">@see EHandleRemoveMode</param>
        public void UnregisterListener(EEventType eventType, OnProcessEventHandle handle, EHandleRemoveMode mode = EHandleRemoveMode.All)
        {
            List<OnProcessEventHandle> handles = GetHandles(eventType);

            if (null != handles && handles.Count > 0)
            {
                switch (mode)
                {
                    case EHandleRemoveMode.All:
                    {
                        for (int i = 0; i < handles.Count;)
                        {
                            OnProcessEventHandle h = handles[i];
                            if (h == handle)
                            {
                                handles.RemoveAt(i);
                                continue;
                            }
                            ++i;
                        }
                        break;
                    }
                    case EHandleRemoveMode.Forward:
                    {
                        for (int i = 0; i < handles.Count; ++i)
                        {
                            OnProcessEventHandle h = handles[i];
                            if (h == handle)
                            {
                                handles.RemoveAt(i);
                                break;
                            }
                        }
                        break;
                    }
                    case EHandleRemoveMode.Backward:
                    {
                        for (int i = handles.Count - 1; i >= 0; --i)
                        {
                            OnProcessEventHandle h = handles[i];
                            if (h == handle)
                            {
                                handles.RemoveAt(i);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="args"></param>
        private void Boardcast(EEventType eventType, EventArgs args)
        {
            List<OnProcessEventHandle> handles = GetHandles(eventType);
            if (null != handles)
            {
                boardcastingHandles.Clear();
                if (boardcastingHandles.Capacity < handles.Count)
                {
                    boardcastingHandles.Capacity = handles.Count;
                }
                boardcastingHandles.AddRange(handles);

                foreach (OnProcessEventHandle h in boardcastingHandles)
                {
                    h.Invoke(eventType, args);
                }
            }
        }

        public void Process(EEventType eventType)
        {
            Process(eventType, null);
        }

        /// <summary>
        /// 处理事件
        /// 将事件监听的最后一个(栈顶)delegate唤醒处理
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="args"></param>
        public void Process(EEventType eventType, EventArgs args)
        {
            List<OnProcessEventHandle> handles = GetHandles(eventType);

            if (null != handles && handles.Count > 0)
            {
                OnProcessEventHandle handle = handles[^1];
                handle.Invoke(eventType, args);
            }
        }

        public void Invoke(EEventType eventType)
        {
            Invoke(eventType, null);
        }

        /// <summary>
        /// 调用事件
        /// 目前直接采取广播行动
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="args"></param>
        public void Invoke(EEventType eventType, EventArgs args)
        {
            Boardcast(eventType, args);
        }
        #endregion

        #region Input
        /// <summary>
        /// 请求退出
        /// </summary>
        private void ApplyEscape()
        {
            this.Process(EEventType.Escape);
        }
        #endregion
    }
}
