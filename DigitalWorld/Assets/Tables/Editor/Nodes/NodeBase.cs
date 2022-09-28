using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Table.Editor
{
    internal class NodeBase
    {
        #region Params
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        protected static System.Type[] typeArray = null;
        protected static string[] typeDisplayArray = null;

        public bool IsEditing { get => isEditing; set => isEditing = value; }
        protected bool isEditing = false;
        #endregion

        #region GUI
        protected virtual void OnGUIName()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(Name, labelStyle, GUILayout.MaxWidth(160));
        }

        /// <summary>
        /// GUI渲染标题
        /// 标题包含各类信息 横向展示
        /// 开关 索引 类型名 名字 以及标题信息
        /// 接下来还有快捷操作按钮集合
        /// 标题底层覆盖函数 用来实现点击开关
        /// </summary>
        public virtual void OnGUITitle()
        {
            
        }

        protected virtual void OnGUIEditing()
        {

        }

        public void OnGUI()
        {
            OnGUITitle();
            if (this.IsEditing)
            {
                OnGUIEditing();
            }
        }
        #endregion

        #region Listen
        protected virtual void OnClickTitle(EditorGUILayout.HorizontalScope horizontalScope)
        {
            isEditing = !isEditing;
        }
        #endregion

        #region Utility
        public static Type[] TypeArray
        {
            get
            {
                if (null == typeArray)
                {
                    List<Type> typeList = new List<Type>();
                    typeList = Logic.Utility.GetUnderlyingTypes(typeList);
                    typeList = Logic.Utility.GetPublicEnumTypes(typeList);
                    typeArray = typeList.ToArray();
                }
                return typeArray;
            }
        }

        public static string[] TypeDisplayArray
        {
            get
            {
                if (null == typeDisplayArray)
                {
                    Type[] types = TypeArray;
                    typeDisplayArray = new string[types.Length];
                    for (int i = 0; i < types.Length; ++i)
                    {
                        typeDisplayArray[i] = types[i].FullName.Replace('.', '/');
                    }
                }
                return typeDisplayArray;
            }
        }

        public static string FindTypeName(int index)
        {
            Type type = FindType(index);
            if (null == type)
                return String.Empty;

            return type.FullName;
        }

        public static Type FindType(int index)
        {
            if (index < 0 || index >= TypeArray.Length)
                return TypeArray[0];

            return TypeArray[index];
        }

        public static int FindTypeIndex(string v)
        {
            int index = 0;

            Type[] types = TypeArray;

            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i].FullName == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion

        #region Serialize
        public virtual void Serialize(XmlElement root)
        {
            root.SetAttribute("name", Name);
            root.SetAttribute("desc", Description);
        }

        public virtual void Deserialize(XmlElement root)
        {
            Name = root.GetAttribute("name");
            Description = root.GetAttribute("desc");
        }
        #endregion
    }
}
