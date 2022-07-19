using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    /// <summary>
    /// 创建行为
    /// </summary>
    internal class LogicBehaviourCreateWindow : EditorWindow
    {
        #region Params
        /// <summary>
        /// 目标文件夹路径
        /// </summary>
        private string targetFolderPath;


        #endregion

        #region Common
        public void Show(string path)
        {
            this.targetFolderPath = path;
        }
        #endregion
    }
}
