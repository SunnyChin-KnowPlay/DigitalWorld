using System.Xml;
using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    /// <summary>
    /// 创建行为
    /// </summary>
    internal class LogicLevelCreateWizard : ScriptableWizard
    {
        #region Params
        /// <summary>
        /// 目标文件夹路径
        /// </summary>
        private string targetFolderPath;

        /// <summary>
        /// 关卡名
        /// </summary>
        public string levelName;
        #endregion

        #region Window
        public static LogicLevelCreateWizard DisplayWizard(string path)
        {
            LogicLevelCreateWizard window = ScriptableWizard.DisplayWizard<LogicLevelCreateWizard>("创建关卡");
            if (null != window)
            {
                window.Show(path);
            }
            return window;
        }
        #endregion

        #region Common
        public void Show(string path)
        {
            this.targetFolderPath = path;
        }

        public static void CreateLevel(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            DisplayWizard(folderPath);
        }
        #endregion

        #region OnGUI
        private void OnWizardCreate()
        {
            string fullPath = targetFolderPath;

            string relativeFilePath = fullPath[(Utility.ConfigsPath.Length + 1)..];

            Level level = new Level
            {
                Name = this.levelName,
                RelativeFolderPath = relativeFilePath
            };

            level.Save();
        }
        #endregion

        #region Menus
        [MenuItem("Assets/Logic/Create/Level", priority = 2)]
        private static void CreateLevel()
        {
            string selectFolderPath = Utility.GetSelectionFolderPath();
            if (string.IsNullOrEmpty(selectFolderPath))
            {
                return;
            }

            DisplayWizard(selectFolderPath);
        }
        #endregion
    }
}
