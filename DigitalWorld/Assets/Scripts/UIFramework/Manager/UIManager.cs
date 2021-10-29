using DigitalWorld.Asset;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalWorld.UI
{
    public sealed class UIManager : MonoBehaviour
    {
        #region Params
        private EventSystem eventSystem = null;

        /// <summary>
        /// 面板词典 面板都是唯一的
        /// </summary>
        private Dictionary<string, Container> panels = new Dictionary<string, Container>();
        #endregion

        #region Instance
        private static bool isDestroying = false;
        private const string uiRootPath = "UI/Root/UIRoot";

        private static UIManager instance = null;

        public static UIManager Instance
        {
            get
            {
                if (null == instance && !isDestroying)
                {
                    GameObject obj = AssetManager.Instance.LoadAsset<GameObject>(uiRootPath);
                    if (null != obj)
                    {
                        GameObject rootObj = GameObject.Instantiate(obj);
                        if (null != rootObj)
                        {
                            instance = rootObj.GetComponent<UIManager>();
                            if (null == instance)
                            {
                                instance = rootObj.AddComponent<UIManager>();
                            }

                            if (null != instance)
                            {
                                rootObj.name = "UIRoot";
                                GameObject.DontDestroyOnLoad(rootObj);
                            }
                            else
                            {
                                GameObject.Destroy(rootObj);
                            }
                        }
                    }
                }
                return instance;
            }
        }

        public static void DestroyInstance()
        {
            if (null != instance && !isDestroying)
            {
                GameObject go = instance.gameObject;
                isDestroying = true;

                if (Application.isPlaying)
                {
                    GameObject.Destroy(go);
                }
                else
                {
                    GameObject.DestroyImmediate(go);
                }
                instance = null;
                isDestroying = false;
            }
        }
        #endregion

        private void Awake()
        {
            if (null != instance && instance.gameObject != this.gameObject)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(this.gameObject);
                }
                else
                {
                    GameObject.DestroyImmediate(this.gameObject);
                }
                return;
            }

            if (null == panels)
                panels = new Dictionary<string, Container>();
            else
                panels.Clear();

            this.InitializeEventSystem();
            this.InitializeCanvases();

        }

        private void OnDestroy()
        {

        }

        #region Initialize


        private void InitializeEventSystem()
        {
            eventSystem = this.GetComponentInChildren<EventSystem>(true);
            if (null == eventSystem)
            {
                GameObject obj = new GameObject("EventSystem");
                eventSystem = obj.AddComponent<EventSystem>();
                obj.AddComponent<StandaloneInputModule>();
                obj.transform.SetParent(this.transform);
            }
        }

        private void InitializeCanvases()
        {

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
