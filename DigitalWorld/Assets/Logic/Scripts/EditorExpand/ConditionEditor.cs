using Dream.FixMath;
using System;
using System.Reflection;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class ConditionBase
    {
#if UNITY_EDITOR
        #region Params
        protected static string[] OperatorStr = new string[6] { "=", "!=", ">", ">=", "<", "<=", };
        #endregion

        #region Common
        protected string GetOperatorStr(ECheckOperator oper)
        {
            return oper switch
            {
                ECheckOperator.Equal => "=",
                ECheckOperator.NotEqual => "!=",
                ECheckOperator.GreaterThan => ">",
                ECheckOperator.GreaterThanOrEquipTo => ">=",
                ECheckOperator.LessThan => "<",
                ECheckOperator.LessThanOrEquipTo => "<=",
                _ => "",
            };
        }
        #endregion

        #region GUI
        protected override void OnGUITitlePropertiesInfo()
        {
            System.Type type = this.GetType();
            StringBuilder title = new StringBuilder();

            title.Append("<color=#50AFCCFF>Properties:</color>(");

            // 这里获取到了所有的字段 把他们一个一个的显示出来
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < fields.Length; ++i)
            {
                FieldInfo field = fields[i];
                if (field.IsFamily)
                    continue;

                string fieldName = field.Name;

                string operStr = this.GetOperatorStr(this.GetOper(i));
                title.AppendFormat("<color=#59E323FF>{0}</color> {1} <color=#F2660BFF>{2}</color>", fieldName, operStr, field.GetValue(this)?.ToString());
                if (i < fields.Length - 1)
                {
                    title.Append(", ");
                }
            }

            title.Append(")");
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };
            EditorGUILayout.SelectableLabel(title.ToString(), labelStyle, GUILayout.MaxHeight(16));
        }

        protected override void OnGUIEditingField(FieldInfo field)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            EditorGUILayout.BeginHorizontal(style);

            bool ret = OnGUICustomEditingField(field);
            if (!ret)
            {
                if (field.FieldType == typeof(int))
                {
                    int v = (int)field.GetValue(this);
                    v = EditorGUILayout.IntField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(float))
                {
                    float v = (float)field.GetValue(this);
                    v = EditorGUILayout.FloatField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(bool))
                {
                    bool v = (bool)field.GetValue(this);
                    v = EditorGUILayout.Toggle(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(string))
                {
                    string v = (string)field.GetValue(this);
                    v = EditorGUILayout.TextField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(FixVector3))
                {
                    FixVector3 v = (FixVector3)field.GetValue(this);
                    Vector3Int vi = new Vector3Int(v.x, v.y, v.z);
                    vi = EditorGUILayout.Vector3IntField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), vi);
                    v = new FixVector3(vi.x, vi.y, vi.z);
                    field.SetValue(this, v);
                }
                else if (field.FieldType == typeof(Color))
                {
                    Color v = (Color)field.GetValue(this);
                    v = EditorGUILayout.ColorField(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);
                    field.SetValue(this, v);
                }
                else if (field.FieldType.BaseType == typeof(Enum))
                {
                    Enum v = (Enum)field.GetValue(this);
                    v = EditorGUILayout.EnumPopup(new GUIContent(field.Name, this.GetFieldDesc(field.Name)), v);

                    field.SetValue(this, v);
                }

            }

            ECheckOperator oper = GetOperatorFromField(field.Name);
            ECheckOperator newOper = (ECheckOperator)EditorGUILayout.Popup((int)oper, OperatorStr, GUILayout.MaxWidth(40));
            if (newOper != oper)
                SetOperatorByField(field.Name, newOper);

            EditorGUILayout.EndHorizontal();
        }
        #endregion
#endif
    }
}
