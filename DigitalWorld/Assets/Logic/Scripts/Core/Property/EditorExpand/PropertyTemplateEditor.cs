#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using Dream.FixMath;

namespace DigitalWorld.Logic.Properties
{
    public partial class PropertyTemplate<T>
    {
        #region Params

        #endregion

        #region GUI
        public delegate void OnDrawEnumTitleValueHandle(GUILayoutOption option);
        private Dictionary<Type, OnDrawEnumTitleValueHandle> guiEnumTitleValues = null;
        public virtual Dictionary<Type, OnDrawEnumTitleValueHandle> GUIEnumTitleValues
        {
            get
            {
                if (null == guiEnumTitleValues)
                {
                    guiEnumTitleValues = new Dictionary<Type, OnDrawEnumTitleValueHandle>
                    {
                        { typeof(ENodeType), OnDrawEnumTitleValue },
                    };
                }
                return guiEnumTitleValues;
            }
        }

        protected override void OnGUITitleContent()
        {
            base.OnGUITitleContent();

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };

            labelStyle.normal.textColor = new Color(42 / 255f, 227 / 255f, 54 / 255f, 1);

            EditorGUILayout.LabelField(new GUIContent("Value:"), labelStyle, GUILayout.Width(40));
            OnGUITitleValue();

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_eyeDropper.Large"), Utility.StyleD__MenuIconButton))
            {

            }
        }

        protected virtual void OnGUITitleValue()
        {
            using (EditorGUI.DisabledGroupScope d = new EditorGUI.DisabledGroupScope(!this.IsCanForceSetValue))
            {
                GUILayoutOption option = GUILayout.Width(300);
                if (typeof(T) == typeof(int))
                {
                    this.Value = (T)Convert.ChangeType(EditorGUILayout.IntField(Convert.ToInt32(Value), option), typeof(T));
                }
                else if (typeof(T) == typeof(float))
                {
                    this.Value = (T)Convert.ChangeType(EditorGUILayout.FloatField(Convert.ToSingle(Value), option), typeof(T));
                }
                else if (typeof(T) == typeof(bool))
                {
                    this.Value = (T)Convert.ChangeType(EditorGUILayout.Toggle(Convert.ToBoolean(Value), option), typeof(T));
                }
                else if (typeof(T) == typeof(string))
                {
                    this.Value = (T)Convert.ChangeType(EditorGUILayout.TextField(Convert.ToString(Value), option), typeof(T));
                }
                else if (typeof(T) == typeof(FixFactor))
                {
                    FixFactor v = (FixFactor)Convert.ChangeType(Value, typeof(FixFactor));
                    Vector2Int vi = new Vector2Int((int)v.nom, (int)v.den);
                    vi = EditorGUILayout.Vector2IntField(new GUIContent(""), vi, option);
                    this.Value = (T)Convert.ChangeType(new FixFactor(vi.x, vi.y), typeof(T));
                }
                else if (typeof(T) == typeof(FixVector2))
                {
                    FixVector2 v = (FixVector2)Convert.ChangeType(Value, typeof(FixVector2));
                    Vector2 vi = new Vector2(v.X, v.Y);
                    vi = EditorGUILayout.Vector2Field(new GUIContent(""), vi, option);
                    this.Value = (T)Convert.ChangeType(new FixVector2(vi.x, vi.y), typeof(T));
                }
                else if (typeof(T) == typeof(FixVector3))
                {
                    FixVector3 v = (FixVector3)Convert.ChangeType(Value, typeof(FixVector3));
                    Vector3 vi = new Vector3(v.X, v.Y, v.Z);
                    vi = EditorGUILayout.Vector3Field(new GUIContent(""), vi, option);
                    this.Value = (T)Convert.ChangeType(new FixVector3(vi.x, vi.y, vi.z), typeof(T));
                }
                else if (typeof(T) == typeof(Color))
                {
                    Color v = (Color)Convert.ChangeType(Value, typeof(Color));
                    v = EditorGUILayout.ColorField(new GUIContent(""), v, option);
                    this.Value = (T)Convert.ChangeType(v, typeof(T));

                }
                else if (typeof(T).IsEnum)
                {
                    OnDrawEnumTitleValue(option);
                }
            }
        }

        protected virtual void OnDrawEnumTitleValue(GUILayoutOption option)
        {
            bool ret = GUIEnumTitleValues.TryGetValue(typeof(T), out OnDrawEnumTitleValueHandle handle);
            if (ret && null != handle)
            {
                handle.Invoke(option);
            }
            else
            {
                System.Enum v = (System.Enum)Convert.ChangeType(Value, typeof(Enum));
                v = EditorGUILayout.EnumPopup(v, option);
                this.Value = (T)Convert.ChangeType(v, typeof(T));
            }
        }

        protected virtual void OnDrawTitleValueNodeType(T value)
        {
            ENodeType v = (ENodeType)Convert.ChangeType(value, typeof(T));
            GUIStyle style = new GUIStyle(GetGUITitleTypeNameStyle(v));
            EditorGUILayout.EnumPopup(v, style);
        }

        protected override void OnGUITitleTypeName()
        {
            GUIStyle style = GetGUITitleTypeNameStyle(this.NodeType);

            System.Type rootType = typeof(T);
            System.Type[] types = PropertyHelper.GetPropertyTypes(rootType);
            if (null == types || types.Length < 1)
            {
                EditorGUILayout.LabelField(this.LocalTypeName, style, GUILayout.Width(240));
            }
            else
            {
                int selectedIndex = PropertyHelper.GetTypeIndex(rootType, this.GetType());
                int newIndex = EditorGUILayout.Popup(selectedIndex, PropertyHelper.GetPropertyLocalTypeNames(rootType), style, GUILayout.Width(240));
                if (newIndex != selectedIndex)
                {
                    System.Type newType = PropertyHelper.GetPropertyType(rootType, newIndex);
                    if (null != newType)
                    {
                        PropertyBase newProperty = Utility.CreateNewProperty(newType);
                        if (null != newProperty)
                        {
                            int nodeIndex = this.Index;
                            NodeBase parent = this.Parent;
                            this.SetParent(null);
                            newProperty.Name = this.Name;
                            newProperty.SetParent(parent, nodeIndex);
                        }
                    }
                }
            }
        }

        public override string GetParamsPreviewString()
        {
            StringBuilder title = new StringBuilder();
            title.Append("<color=#2ACDE2FF>[</color>");
            title.Append(Value);
            title.Append("<color=#2ACDE2FF>|</color>");

            Type type = this.GetType();
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            List<string> fieldList = new List<string>();
            for (int i = 0; i < fields.Length; ++i)
            {
                FieldInfo field = fields[i];
                if (field.IsFamily)
                    continue;

                string fieldName = field.Name;
                fieldList.Add(string.Format("<color=#2AE336FF>{0}</color><color=#FFFFFFFF>=</color><color=#FFFFFFFF>{1}</color>", fieldName, field.GetValue(this)?.ToString()));
            }

            if (fieldList.Count > 0)
            {
                string ret = string.Join(",", fieldList);
                title.Append(ret);
            }

            title.Append("<color=#2ACDE2FF>]</color>");
            return title.ToString();

        }
        #endregion
    }
}
#endif