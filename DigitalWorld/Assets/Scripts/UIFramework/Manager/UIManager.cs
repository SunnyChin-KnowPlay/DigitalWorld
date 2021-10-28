using DigitalWorld.Asset;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalWorld.UI
{
    public sealed class UIManager : DreamEngine.Singleton<UIManager>
    {
        private GameObject rootObject = null;
        private EventSystem eventSystem = null;
        private Camera rootCamera = null;

        /// <summary>
        /// 面板词典 面板都是唯一的
        /// </summary>
        private Dictionary<string, Container> panels = new Dictionary<string, Container>();

        protected override void Awake()
        {
            base.Awake();

            if (null == panels)
                panels = new Dictionary<string, Container>();
            else
                panels.Clear();

            this.InitializeRoot();
            this.InitializeEventSystem();
            this.InitializeCamera();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (null != rootObject)
            {
                GameObject.Destroy(rootObject);
                rootObject = null;
            }
        }

        #region Initialize
        private void InitializeRoot()
        {
            GameObject rootObj = Resources.Load("UI/Root/UIRoot") as GameObject;

            rootObject = null != rootObj ? GameObject.Instantiate(rootObj) : new GameObject("UIRoot");
            rootObj.name = "UIRoot";
            GameObject.DontDestroyOnLoad(rootObject);
        }

        private void InitializeEventSystem()
        {
            eventSystem = rootObject.GetComponentInChildren<EventSystem>(true);
            if (null == eventSystem)
            {
                GameObject obj = new GameObject("EventSystem");
                eventSystem = obj.AddComponent<EventSystem>();
                obj.AddComponent<StandaloneInputModule>();
                obj.transform.SetParent(rootObject.transform);
            }
        }

        private void InitializeCamera()
        {
            Canvas canvas = rootObject.GetComponent<Canvas>();
            if (null != canvas)
            {
                this.rootCamera = canvas.worldCamera;
            }
        }
        #endregion

        #region Container
        public GameObject CreateContainer(string path)
        {
            GameObject gameObject = null;
            string fullPath = path + ".prefab";
            UnityEngine.Object target = AssetManager.Instance.LoadAsset<UnityEngine.Object>(fullPath);

            if (null != target)
            {
                gameObject = (GameObject)UnityEngine.GameObject.Instantiate(target) as GameObject;
            }
            return gameObject;
        }
        #endregion

        #region Panel
        private Container GetPanel(string key)
        {
            this.panels.TryGetValue(key, out Container container);
            return container;
        }

        public Container LoadPanel(string path)
        {
            Container container = this.GetPanel(path);
            if (null != container)
                return container;

            GameObject go = this.CreateContainer(path);
            if (null == go)
                return null;

            container = go.GetComponent<Container>();
            if (null == container)
            {
                container = go.AddComponent<Container>();
            }

            if (null != container)
            {
                this.panels.Add(path, container);
            }

            return container;
        }
        #endregion

        #region Utility
        /// <summary>
        /// 递归设置Transform及子节点的layer
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="layerMask"></param>
        public static void SetLayerWithChildren(Transform trans, int layerMask)
        {
            if (layerMask < 0)
            {
                return;
            }
            trans.gameObject.layer = layerMask;
            for (int i = 0; i < trans.childCount; ++i)
            {
                SetLayerWithChildren(trans.GetChild(i), layerMask);
            }
        }
        #endregion
    }
}
