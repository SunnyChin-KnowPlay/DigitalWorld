using UnityEngine;

namespace DreamEngine
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public enum EnumState
        {
            None = 0,
            /// <summary>
            /// 释放中
            /// </summary>
            Destroying,
            /// <summary>
            /// 运行中
            /// </summary>
            Living,
        }

        #region Params
        private const string singletonRootName = "Singletons";
        public static EnumState currentState = EnumState.None;
        //private static EnumState _st = EDestroySt.E_DST_DESTROYED;
        #endregion

        #region Instance
        private static T instance = null;

        public static T Instance
        {
            get => GetInstance();
        }

        private static GameObject RootObject
        {
            get
            {
                GameObject rootObj = GameObject.Find(singletonRootName);
                if (null == rootObj)
                {
                    rootObj = new GameObject(singletonRootName);
                    if (Application.isPlaying)
                    {
                        GameObject.DontDestroyOnLoad(rootObj);
                    }
                }
                return rootObj;
            }
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            if (null == instance && currentState == EnumState.None)
            {
                System.Type type = typeof(T);

                instance = FindObjectOfType(type) as T;
                if (null == instance)
                {
                    string name = string.Format("Singleton of {0}", type.Name);
                    GameObject go = new GameObject(name);
                    instance = go.AddComponent<T>();

                    if (instance)
                    {
                        GameObject rootObj = RootObject;
                        if (rootObj != null)
                        {
                            go.transform.SetParent(rootObj.transform);
                        }

                        currentState = EnumState.Living;
                    }
                    else
                    {
                        GameObject.Destroy(go);
                    }
                }
                else
                {
                    currentState = EnumState.Living;
                }
            }

            return instance;
        }

        public static void DestroyInstance()
        {
            if (null != instance && currentState == EnumState.Living)
            {
                currentState = EnumState.Destroying;
                GameObject go = instance.gameObject;

                if (Application.isPlaying)
                {
                    GameObject.Destroy(go);
                }
                else
                {
                    GameObject.DestroyImmediate(go);
                }

                currentState = EnumState.None;
            }
        }
        #endregion

        #region Mono
        protected virtual void Awake()
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

            if (Application.isPlaying)
            {
                GameObject.DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (null != instance && instance.gameObject == this.gameObject)
            {
                instance = null;
            }
        }
        #endregion
    }

}

