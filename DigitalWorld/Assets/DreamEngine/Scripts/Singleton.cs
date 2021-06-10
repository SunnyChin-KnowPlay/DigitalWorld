using UnityEngine;

namespace DreamEngine
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        #region Instance
        private static T _instance = null;

        public static T instance
        {
            get { return _instance; }
        }
        #endregion


        /// <summary>
        /// ´´½¨ÊµÀý
        /// </summary>
        /// <returns></returns>
        public static T CreateInstance()
        {
            if (null != _instance)
                return _instance;

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

