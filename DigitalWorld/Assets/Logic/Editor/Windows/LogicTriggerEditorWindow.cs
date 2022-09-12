using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicTriggerEditorWindow : EditorWindow
    {
        #region Params
        private Vector2 fileListScrollViewPos = Vector2.zero;

        /// <summary>
        /// 当前正在编辑的触发器
        /// </summary>
        private Trigger currentTrigger = null;

        /// <summary>
        /// 所有正在编辑的触发器
        /// </summary>
        private readonly static Dictionary<string, LogicTriggerEditorWindow> editingTriggers = new Dictionary<string, LogicTriggerEditorWindow>();
        #endregion

        #region Common
        /// <summary>
        /// 检查是否有已打开正在编辑的窗口 如果有的话 直接还出去
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        internal static bool CheckHasEditing(string relativePath, out LogicTriggerEditorWindow window)
        {
            if (editingTriggers.ContainsKey(relativePath))
            {
                window = editingTriggers[relativePath];
                return true;
            }

            window = null;
            return false;
        }

        private void OnDestroy()
        {
            if (null != this.currentTrigger)
            {
                editingTriggers.Remove(currentTrigger.RelativeAssetFilePath);
            }
        }
        #endregion

        #region GUI
        private void SetTitle(string title)
        {
            this.titleContent = new GUIContent(title);
        }

        public void Show(Trigger trigger)
        {
            base.Show();
            string fileName = System.IO.Path.GetFileNameWithoutExtension(trigger.RelativeAssetFilePath);
            this.SetTitle(fileName);

            this.currentTrigger = trigger;
            editingTriggers.Add(trigger.RelativeAssetFilePath, this);
        }

        private void OnGUI()
        {
            if (null != currentTrigger)
            {
                fileListScrollViewPos = EditorGUILayout.BeginScrollView(fileListScrollViewPos);
                this.currentTrigger.OnGUI();
                EditorGUILayout.EndScrollView();

                OnGUIBottomMenus();
            }
        }

        /// <summary>
        /// GUI底部菜单
        /// </summary>
        private void OnGUIBottomMenus()
        {
            GUIStyle style = new GUIStyle("Tooltip");
            EditorGUILayout.BeginHorizontal(style);
            if (GUILayout.Button("Save"))
            {
                this.currentTrigger.Save();
            }

            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion


    }
}
