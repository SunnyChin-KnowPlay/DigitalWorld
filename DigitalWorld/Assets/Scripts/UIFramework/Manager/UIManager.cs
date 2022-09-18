using DigitalWorld.Asset;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalWorld.UI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public sealed class UIManager : MonoBehaviour
    {
        #region Params
        public EventSystem eventSystem = null;
        public Camera uiCamera = null;

        /// <summary>
        /// 界面词典 界面有唯一性
        /// key:界面路径
        /// </summary>
        private readonly Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

        private Transform trans = null;
        #endregion

        #region Instance
        private static bool isDestroying = false;
        private const string uiRootPath = "Assets/Res/UI/Root/UIRoot.prefab";

        private static UIManager instance = null;

        public static UIManager Instance
        {
            get
            {
                if (null == instance && !isDestroying)
                {
                    GameObject obj = AssetManager.LoadAsset<GameObject>(uiRootPath);
                    if (null != obj)
                    {
                        GameObject rootObj = GameObject.Instantiate(obj);
                        if (null != rootObj)
                        {
                            if (!rootObj.TryGetComponent<UIManager>(out instance))
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

        #region Mono
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

            trans = this.GetComponent<Transform>();

            this.InitializePanels();

        }

        private void OnDestroy()
        {

        }
        #endregion

        #region Initialize
        private void InitializePanels()
        {
            panels.Clear();
        }

        #endregion

        #region Panel
        /// <summary>
        /// 创建界面
        /// </summary>
        /// <param name="path">界面在资源文件夹下的路径</param>
        /// <returns></returns>
        public GameObject CreatePanel(string path)
        {
            GameObject gameObject = null;
            string fullPath = path;
            UnityEngine.Object target = AssetManager.LoadAsset<UnityEngine.Object>(fullPath);

            if (null != target)
            {
                gameObject = (GameObject)UnityEngine.GameObject.Instantiate(target) as GameObject;
                if (null != gameObject)
                {
                    gameObject.name = target.name;
                }
            }

            return gameObject;
        }

        public async Task<GameObject> CreatePanelAsync(string path)
        {
            GameObject gameObject = null;
            string fullPath = path;
            GameObject target = await AssetManager.LoadAssetAsync<UnityEngine.GameObject>(fullPath);

            if (null != target)
            {
                gameObject = (GameObject)UnityEngine.GameObject.Instantiate(target) as GameObject;
                if (null != gameObject)
                {
                    gameObject.name = target.name;
                }
            }

            return gameObject;
        }

        public void DestroyPanel(GameObject go)
        {
            Destroy(go);
        }

        private GameObject GetPanel(string path)
        {
            this.panels.TryGetValue(path, out GameObject go);
            return go;
        }

        public GameObject LoadPanel(string path)
        {
            GameObject go = GetPanel(path);
            if (null == go)
            {
                go = this.CreatePanel(path);

                if (null != go)
                {
                    this.panels.Add(path, go);
                }
            }

            return go;
        }

        public void UnloadPanel(string path)
        {
            GameObject go = GetPanel(path);

            if (null != go)
            {
                this.panels.Remove(path);
                DestroyPanel(go);
            }
        }

        public void UnloadAllPanels()
        {
            foreach (var kvp in panels)
            {
                DestroyPanel(kvp.Value);
            }
            this.panels.Clear();
        }

        public async Task<GameObject> LoadPanelAsync(string path)
        {
            GameObject go = GetPanel(path);
            if (null == go)
            {
                go = await this.CreatePanelAsync(path);

                if (null != go)
                {
                    this.panels.Add(path, go);
                }
            }

            return go;
        }

        private void SetupPanel(GameObject go)
        {
            go.transform.SetParent(trans, false);

            if (go.TryGetComponent<Canvas>(out Canvas canvas))
            {
                canvas.worldCamera = this.uiCamera;
            }
        }

        public void ShowPanel(string path)
        {
            GameObject go = this.LoadPanel(path);
            if (null != go)
            {
                go.SetActive(true);
                SetupPanel(go);
            }
        }

        public async void ShowPanelAsync<TControl>(string path) where TControl : PanelControl
        {
            GameObject go = await this.LoadPanelAsync(path);
            if (null == go)
                return;

            if (!go.TryGetComponent<PanelControl>(out PanelControl panel))
            {
                go.AddComponent<TControl>();
            }
            else if (panel.GetType() != typeof(TControl))
            {
                GameObject.Destroy(panel);
                go.AddComponent<TControl>();
            }

            go.SetActive(true);

            SetupPanel(go);
        }

        public void ShowPanel<TControl>(string path) where TControl : PanelControl
        {
            GameObject go = this.LoadPanel(path);
            if (null == go)
                return;

            if (!go.TryGetComponent<PanelControl>(out PanelControl panel))
            {
                go.AddComponent<TControl>();
            }
            else if (panel.GetType() != typeof(TControl))
            {
                GameObject.Destroy(panel);
                go.AddComponent<TControl>();
            }

            go.SetActive(true);

            SetupPanel(go);
        }

        public void HidePanel(string path)
        {
            GameObject go = this.GetPanel(path);
            if (null != go)
            {
                go.SetActive(false);
            }
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
