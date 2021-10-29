﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.UI
{
    public class Container : MonoBehaviour
    {
        [HideInInspector]
        public GameObject[] widgetObjects = new GameObject[0];
        [HideInInspector]
        public string[] widgetKeys = new string[0];

        protected RectTransform rectTransform = null;
        /// <summary>
        /// RectTransform emmm....
        /// </summary>
        public RectTransform RectTransform { get => rectTransform; }

        private Dictionary<string, GameObject> widgets = new Dictionary<string, GameObject>();

        protected virtual void Awake()
        {
            rectTransform = this.GetComponent<RectTransform>();

            InitializeWidgets();
        }

        #region Initialize
        protected virtual void InitializeWidgets()
        {
            if (widgetKeys.Length != widgetObjects.Length)
                return;

            int length = widgetKeys.Length;
            this.widgets.Clear();

            for (int i = 0; i < length; ++i)
            {
                this.widgets.Add(widgetKeys[i], widgetObjects[i]);
            }
        }
        #endregion

        #region Get
        public T GetWidget<T>(string name) where T : Component
        {
            this.widgets.TryGetValue(name, out GameObject go);
            if (null == go)
                return null;

            return go.GetComponent<T>();
        }

        public GameObject GetObj(string name)
        {
            this.widgets.TryGetValue(name, out GameObject go);

            return go;
        }
        #endregion
    }
}
