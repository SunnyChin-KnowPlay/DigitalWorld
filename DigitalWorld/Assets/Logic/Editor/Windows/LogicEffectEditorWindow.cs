using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    using static DigitalWorld.Logic.NodeBase;
    internal abstract class LogicEffectEditorWindow : EditorWindow
    {
        protected enum EShowMode
        {
            Add = 0,
            Edit = 1,
        }


        #region Params
        protected EShowMode currentMode;
        /// <summary>
        /// 父节点
        /// </summary>
        protected Logic.NodeBase parent = null;

        /// <summary>
        /// 初始节点 - 编辑时才有意义 是需要被替换掉的
        /// </summary>
        protected Logic.NodeBase initialNode = null;

        /// <summary>
        /// 当前选择的节点
        /// </summary>
        protected Logic.NodeBase currentNode = null;
      
        protected List<int> typeIndexs = new List<int>();
        protected List<string> typeNames = new List<string>();

        protected string filter = string.Empty;
        #endregion

        #region GUI
        public virtual void Show(Logic.NodeBase parent)
        {
            base.ShowPopup();

            this.currentMode = EShowMode.Add;

            this.parent = parent;
            this.currentNode = null;
            this.initialNode = null;

            this.titleContent.text = GetTitle(this.currentMode);
        }

        public virtual void Show(Logic.NodeBase parent, Logic.NodeBase node)
        {
            base.ShowPopup();


            currentMode = EShowMode.Edit;

            this.parent = parent;
            this.currentNode = node.Clone() as Logic.NodeBase;
            this.initialNode = node;

            this.titleContent.text = GetTitle(this.currentMode);
        }

        protected abstract string GetTitle(EShowMode mode);

        private void OnGUIFilter()
        {
            GUIStyle style = new GUIStyle("Tooltip");
            EditorGUILayout.BeginHorizontal(style);
            EditorGUILayout.LabelField("筛选器");

            GUILayout.FlexibleSpace();

            var TextFieldRoundEdge = new GUIStyle("SearchTextField");
            TextFieldRoundEdge.fontSize = 12;
            filter = EditorGUILayout.TextField(filter, TextFieldRoundEdge);

            TextFieldRoundEdge.fontSize = 11;
            EditorGUILayout.EndHorizontal();
        }

        private void OnGUIBottomMenus()
        {
            GUIStyle style = new GUIStyle("Tooltip");

            EditorGUILayout.BeginHorizontal(style);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("确定"))
            {
                this.OnClickSave();
            }

            if (GUILayout.Button("取消"))
            {
                this.OnClickCancel();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected abstract void OnGUIBody();

        protected virtual void OnGUI()
        {
            OnGUIFilter();

            OnGUIBody();

            GUILayout.FlexibleSpace();

            OnGUIBottomMenus();
        }
        #endregion

        #region Listen
        protected void OnClickSave()
        {
            if (null == this.currentNode)
                return;

            if (this.currentMode == EShowMode.Add)
            {
                this.currentNode.SetParent(this.parent);
            }
            else
            {
                if (this.currentNode!= initialNode)
                {
                    int index = initialNode.Index;
                    initialNode.SetParent(null);
                    currentNode.SetParent(this.parent);
                }
            }

            if (null != this.currentNode)
                this.currentNode.SetDirty();

            this.Close();
        }

        private void OnClickCancel()
        {
            this.Close();
        }
        #endregion
    }
}
