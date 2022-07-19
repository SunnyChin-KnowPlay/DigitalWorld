using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    internal static class Utility
    {
        /// <summary>
        /// 获取当前选择的文件夹路径
        /// </summary>
        /// <returns></returns>
        internal static string GetSelectionFolderPath()
        {
            string[] guid = Selection.assetGUIDs;
            if (null == guid || guid.Length <= 0)
            {
                UnityEngine.Debug.LogError("请先选择一个文件/文件夹");
                return string.Empty;
            }

            return AssetDatabase.GUIDToAssetPath(guid[0]);
        }
    }
}
