using System.IO;
using UnityEngine;

namespace DigitalWorld.Utilities
{
    public static class Utility
    {
        private const string comKey = "com.DigitalWorld";

        private static string GetFullKey(string key)
        {
            return string.Format("{0}.{1}", comKey, key);
        }

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            if (string.IsNullOrEmpty(key)) return defaultValue;
            return PlayerPrefs.GetFloat(GetFullKey(key), defaultValue);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(key)) return defaultValue;
            return PlayerPrefs.GetInt(GetFullKey(key), defaultValue);
        }

        public static string GetString(string key, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(key)) return defaultValue;
            return PlayerPrefs.GetString(GetFullKey(key), defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            if (string.IsNullOrEmpty(key)) return;
            PlayerPrefs.SetFloat(GetFullKey(key), value);
        }

        public static void SetInt(string key, int value)
        {
            if (string.IsNullOrEmpty(key)) return;
            PlayerPrefs.SetInt(GetFullKey(key), value);
        }

        public static void SetString(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            PlayerPrefs.SetString(GetFullKey(key), value);
        }

        /// <summary>
        /// 设置"默认的"值，仅当缓存中没有该key时才起效
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetDefaultString(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            string fullKey = GetFullKey(key);
            if (PlayerPrefs.HasKey(fullKey))
                return;

            PlayerPrefs.SetString(fullKey, value);
        }

        /// <summary>
        /// 设置"默认的"值，仅当缓存中没有该key时才起效
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetDefaultFloat(string key, float value)
        {
            if (string.IsNullOrEmpty(key)) return;
            string fullKey = GetFullKey(key);
            if (PlayerPrefs.HasKey(fullKey))
                return;

            PlayerPrefs.SetFloat(fullKey, value);
        }

        /// <summary>
        /// 设置"默认的"值，仅当缓存中没有该key时才起效
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetDefaultInt(string key, int value)
        {
            if (string.IsNullOrEmpty(key)) return;
            string fullKey = GetFullKey(key);
            if (PlayerPrefs.HasKey(fullKey))
                return;

            PlayerPrefs.SetInt(fullKey, value);
        }

        /// <summary>
        /// 获取项目文件路径 Application.dataPath移除"/Assets"
        /// </summary>
        /// <returns></returns>
        public static string GetProjectDataPath()
        {
            string p = Application.dataPath.Replace("/Assets", "");
            return p;
        }

        public static void ClearDirectory(string path)
        {
            if (!Directory.Exists(path))
                return;

            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; ++i)
            {
                File.Delete(files[i]);
            }

            string[] directories = Directory.GetDirectories(path);
            for (int i = 0; i < directories.Length; ++i)
            {
                if (Directory.Exists(directories[i]))
                {
                    ClearDirectory(directories[i]);
                    Directory.Delete(directories[i]);
                }
            }

        }
    }
}

