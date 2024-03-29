﻿using UnityEditor;
using DigitalWorld.Logic.Editor;
using static DigitalWorld.Logic.Editor.NodeItem;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicItemCreateWizard : ScriptableWizard
    {
        #region Params
        private OnCallbackModifyItem addHandle;
        private EItemType type;
        public int id;
        public string itemName;
        public string itemDescription;
        /// <summary>
        /// 属性专用的
        /// </summary>
        private string valueType;
        #endregion

        #region Window
        public static LogicItemCreateWizard DisplayWizard(EItemType type, OnCallbackModifyItem callback, int id)
        {
            string title = string.Format("Create {0}", type);
            LogicItemCreateWizard window = ScriptableWizard.DisplayWizard<LogicItemCreateWizard>(title);
            if (null != window)
            {
                window.Show(type, callback, id);
            }
            return window;
        }
        #endregion

        #region Common
        public virtual void Show(EItemType type, OnCallbackModifyItem callback, int id)
        {
            this.addHandle = callback;
            this.type = type;
            this.id = id;
            this.itemName = name;
        }
        #endregion

        #region OnGUI
        protected override bool DrawWizardGUI()
        {
            bool flag = base.DrawWizardGUI();
            switch (this.type)
            {
                case EItemType.Property:
                {
                    this.valueType = NodeProperty.FindTypeName(EditorGUILayout.Popup("valueType", NodeProperty.FindTypeIndex(this.valueType), NodeField.TypeDisplayArray));
                    break;
                }
            }
            return flag;
        }

        protected virtual void OnWizardCreate()
        {
            NodeItem exitsNode = null;

            LogicItemsEditorWindow window = EditorWindow.GetWindow<LogicItemsEditorWindow>();

            if (window.NodeController.CheckItemExits(this.type, id, ref exitsNode))
            {
                string title = NodeController.GetTitleWithType(type);
                string v = string.Format("{0}已存在，使用该ID({1})的{0}为({2})", title, id, exitsNode.Name);
                EditorUtility.DisplayDialog("错误，无法保存", v, "Ok");
                return;
            }

            if (window.NodeController.CheckItemExits(this.type, itemName, ref exitsNode))
            {
                string title = NodeController.GetTitleWithType(type);
                string v = string.Format("{0}已存在，使用该名字({1})的{0}为({2})", title, itemName, exitsNode.Name);
                EditorUtility.DisplayDialog("错误，无法保存", v, "Ok");
                return;
            }

            if (null != addHandle)
            {
                NodeItem item = window.NodeController.CreateItem(type);
                item.Id = id;
                item.Name = itemName;
                item.Desc = itemDescription;

                switch (this.type)
                {
                    case EItemType.Property:
                    {
                        if (item is NodeProperty property)
                        {
                            property.ValueType = this.valueType;
                        }
                        break;
                    }
                }

                addHandle.Invoke(this.type, item);
            }

        }
        #endregion
    }
}
