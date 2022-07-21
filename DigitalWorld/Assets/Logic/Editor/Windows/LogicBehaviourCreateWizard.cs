using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    /// <summary>
    /// 创建行为
    /// </summary>
    internal class LogicBehaviourCreateWizard : ScriptableWizard
    {
        #region Params
        /// <summary>
        /// 目标文件夹路径
        /// </summary>
        private string targetFolderPath;

        /// <summary>
        /// 行为名
        /// </summary>
        public string behaviourName;
        #endregion

        #region Window
        public static LogicBehaviourCreateWizard DisplayWizard(string path)
        {
            LogicBehaviourCreateWizard window = ScriptableWizard.DisplayWizard<LogicBehaviourCreateWizard>("Create Behaviour");
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
        #endregion

        #region OnGUI
        private void OnWizardCreate()
        {
            string fullPath = System.IO.Path.Combine(this.targetFolderPath, this.behaviourName);
            fullPath += ".asset";

            if (System.IO.File.Exists(fullPath))
            {
                // 如果已经存在 则先报错忽视
                UnityEngine.Debug.LogError("File is Exits:" + fullPath);
                return;
            }

            //string fileName = "test.xml";
            //string fullPath = System.IO.Path.Combine(selectFolderPath, fileName);

            TextAsset ta = new TextAsset();
            AssetDatabase.CreateAsset(ta, fullPath);
        }
        #endregion
    }
}
