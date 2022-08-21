using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicItemEditorWindow : EditorWindow
    {
        #region Params
        /// <summary>
        /// 当前正在编辑的行为
        /// </summary>
        private NodeItem currentItem = null;

        /// <summary>
        /// 所有正在编辑的行为
        /// </summary>
        private static Dictionary<string, LogicItemEditorWindow> editingItems = new Dictionary<string, LogicItemEditorWindow>();

        #endregion


        #region Common
        /// <summary>
        /// 检查是否有已打开正在编辑的窗口 如果有的话 直接还出去
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        internal static bool CheckHasEditing(string name, out LogicItemEditorWindow window)
        {
            if (editingItems.ContainsKey(name))
            {
                window = editingItems[name];
                return true;
            }

            window = null;
            return false;
        }

        private void OnDestroy()
        {
            if (null != this.currentItem)
            {
                editingItems.Remove(currentItem.Name);
            }
        }

        public void Show(NodeItem item)
        {
            base.Show();

            this.SetTitle(item.Name);

            this.currentItem = item;
            editingItems.Add(item.Name, this);
        }
        #endregion



        #region ONGUI
        private void SetTitle(string title)
        {
            this.titleContent = new GUIContent(title);
        }

        private void OnGUI()
        {

            if (null != currentItem)
            {
                currentItem.OnGUIParams(true);
                currentItem.OnGUIBody();
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Save"))
            {
                this.OnClickSave();
            }

            if (GUILayout.Button("Cancel"))
            {
                this.OnClickCancel();
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Listen
        private void OnClickSave()
        {
            this.currentItem.SetDirty();
            EditorWindow.GetWindow<LogicItemsEditorWindow>().Repaint();

            this.Close();
        }

        private void OnClickCancel()
        {
            this.Close();
        }
        #endregion
    }
}
