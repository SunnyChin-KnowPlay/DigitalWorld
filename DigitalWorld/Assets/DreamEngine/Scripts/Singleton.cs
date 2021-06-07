using UnityEngine;

namespace DreamEngine
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private const string _singletonRootObjectName = "Singletons";

        private static T _instance = null;

        public static T instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// ´´½¨ÊµÀý
        /// </summary>
        /// <returns></returns>
        public static T CreateInstance()
        {
            GameObject root = GameObject.Find(_singletonRootObjectName);

            System.Type type = typeof(T);
            string name = string.Format("Singleton of {0}", type.Name);
            GameObject go = new GameObject(name);
            GameObject.DontDestroyOnLoad(go);

            T com = go.AddComponent<T>();
            return com;
        }

        public static void DestroyInstance()
        {
            if (null != _instance)
            {
                GameObject go = _instance.gameObject;
#if UNITY_EDITOR
                GameObject.DestroyImmediate(go);
#else
                GameObject.Destroy(go);
#endif
            }
        }

        protected virtual void Awake()
        {
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }

}

