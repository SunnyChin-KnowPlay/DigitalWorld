using UnityEngine;

namespace DigitalWorld.Extension.Unity
{
    public static class EComponent
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            if (null == component)
                return null;

            T com = component.GetComponent<T>();
            if (null != com)
                return com;

            com = component.gameObject.AddComponent<T>();
            return com;
        }

        public static T GetOrAddComponent<T>(this GameObject component) where T : Component
        {
            if (null == component)
                return null;

            T com = component.GetComponent<T>();
            if (null != com)
                return com;

            com = component.gameObject.AddComponent<T>();
            return com;
        }
    }
}