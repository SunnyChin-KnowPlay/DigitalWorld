#if UNITY_EDITOR
using System.Reflection;
using System.Text;
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class Effect
    {
#if UNITY_EDITOR
        #region GUI
        public override void OnGUI()
        {
            base.OnGUI();
        }

        protected override void OnGUITitleMenus()
        {
            if (GUILayout.Button("Edit"))
            {
                OnClickEdit();
            }

            base.OnGUITitleMenus();
        }

        protected override void OnGUITitleInfo()
        {
            base.OnGUITitleInfo();

            OnGUITitleRequirementsInfo();
            OnGUITitlePropertiesInfo();
        }

        protected virtual void OnGUITitleRequirementsInfo()
        {
            StringBuilder title = new StringBuilder();

            title.Append("<color=#50AFCCFF>Requirements:</color>(");

            int index = 0;
            foreach (var kvp in this._requirements)
            {
                title.AppendFormat("<color=#59E323FF>{0}</color> = <color=#F2660BFF>{1}</color>", kvp.Key, kvp.Value);

                index++;
                if (index < this._requirements.Count)
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

        protected virtual void OnGUITitlePropertiesInfo()
        {

            StringBuilder title = new StringBuilder();
            title.Append("<color=#50AFCCFF>Properties:</color>(");

            System.Type type = this.GetType();
            // 这里获取到了所有的字段 把他们一个一个的显示出来
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

            string fieldName = "";
            for (int i = 0; i < fields.Length; ++i)
            {
                FieldInfo field = fields[i];
                if (field.IsFamily)
                    continue;

                fieldName = field.Name;
                title.AppendFormat("<color=#59E323FF>{0}</color> = <color=#F2660BFF>{1}</color>", fieldName, field.GetValue(this)?.ToString());
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
        #endregion

        #region Listen
        protected virtual void OnClickEdit()
        {
            LogicHelper.ApplyEditNode(this.NodeType, this.Parent, this);
        }
        #endregion
#endif
    }
}
