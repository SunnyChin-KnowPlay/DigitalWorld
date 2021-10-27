using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.UI
{
    public class UIContainer : MonoBehaviour
    {
        [HideInInspector]
        public GameObject[] widgetObjects = new GameObject[0];
        [HideInInspector]
        public string[] widgetKeys = new string[0];

        private Dictionary<string, GameObject> widgetDict = new Dictionary<string, GameObject>();

        protected virtual void Awake()
        {
            InitializeWidgets();
        }

        #region Initialize
        protected virtual void InitializeWidgets()
        {
            int length = widgetKeys.Length;
            this.widgetDict.Clear();

            for (int i = 0; i < length; ++i)
            {
                this.widgetDict.Add(widgetKeys[i], widgetObjects[i]);
            }
        }
        #endregion

        #region Get
        public T GetWidget<T>(string name) where T : Component
        {
            GameObject go = null;
            this.widgetDict.TryGetValue(name, out go);
            if (null == go)
                return null;

            return go.GetComponent<T>();
        }

        public GameObject GetObj(string name)
        {
            GameObject go = null;
            this.widgetDict.TryGetValue(name, out go);

            return go;
        }
        #endregion
    }
}
