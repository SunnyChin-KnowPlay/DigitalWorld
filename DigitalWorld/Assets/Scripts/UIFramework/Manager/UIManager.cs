using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalWorld.UI
{
    public sealed class UIManager : DreamEngine.Singleton<UIManager>
    {
        private GameObject rootObject = null;
        private EventSystem eventSystem = null;
        private Camera rootCamera = null;

        protected override void Awake()
        {
            base.Awake();

            this.InitializeRoot();
            this.InitializeEventSystem();
            this.InitializeCamera();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
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
    }
}
