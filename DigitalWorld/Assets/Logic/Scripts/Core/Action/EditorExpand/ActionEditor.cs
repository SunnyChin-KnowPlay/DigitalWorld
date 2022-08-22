#if UNITY_EDITOR
using Dream.FixMath;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;

namespace DigitalWorld.Logic.Actions
{
    public partial class ActionBase
    {
#if UNITY_EDITOR
        #region Params
        protected ReorderableList reorderableRequirementList;
        protected ReorderableList reorderablePropertiesList;
        protected List<string> brotherNames = new List<string>();
        protected List<FieldInfo> fieldInfos = null;


        #endregion

        #region Common
        public ActionBase()
        {
            reorderableRequirementList = new ReorderableList(this._requirements, typeof(Requirement))
            {
                drawElementCallback = OnDrawRequiementElement,
                onAddCallback = (list) => OnAddRequiement(),
                onRemoveCallback = (list) => OnRemoveRequiement(),
                drawHeaderCallback = OnDrawRequiementHead,
            };

            reorderablePropertiesList = new ReorderableList(this.FieldInfos, typeof(FieldInfo))
            {
                drawElementCallback = OnDrawPropertyElement,
                drawHeaderCallback = OnDrawPropertyHead,
                displayAdd = false,
                displayRemove = false,
                draggable = false,
            };
        }

        protected List<string> CalculateBrotherNames()
        {
            brotherNames.Clear();

            if (null != this._parent)
            {
                List<NodeBase> children = _parent.Children;
                foreach (NodeBase brother in children)
                {
                    brotherNames.Add(brother.Name);
                }
            }

            return brotherNames;
        }

        protected List<FieldInfo> FieldInfos
        {
            get
            {
                if (null == fieldInfos)
                {
                    System.Type type = this.GetType();
                    // 这里获取到了所有的字段 把他们一个一个的显示出来
                    FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                    fieldInfos = new List<FieldInfo>(fields.Length);
                    fieldInfos.AddRange(fields);
                }
                return fieldInfos;
            }
        }

        protected int GetBrotherIndex(string name)
        {
            int index = -1;

            for (int i = 0; i < brotherNames.Count; ++i)
            {
                string n = brotherNames[i];
                if (n == name)
                {
                    index = i; break;
                }
            }

            return index;
        }

        public string[] GetPropertyNames(List<System.Type> types)
        {
            List<string> classList = new List<string>();

            foreach (System.Type type in types)
            {
                string v = type.ToString();
                classList.Add(v);
            }

            return classList.ToArray();
        }

        public string[] GetPropertyDisplayNames(List<System.Type> types)
        {
            List<string> classList = new List<string>();

            foreach (System.Type type in types)
            {
                string v = type.ToString();
                classList.Add(v.Replace('.', '/'));
            }

            return classList.ToArray();
        }

        public static string FindPropertyType(string[] types, int index)
        {
            if (index < 0 || index >= types.Length)
                return types[0];

            return types[index];
        }

        public static int FindPropertyTypeIndex(string[] types, string v)
        {
            int index = 0;
            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion

        #region GUI
        protected override void OnGUIName()
        {
            base.OnGUIName();
        }

        protected override void OnGUIEditing()
        {
            base.OnGUIEditing();

            OnGUIEditingRequirementsInfo();
            OnGUIEditingPropertiesInfo();
        }

        protected override void OnGUITitleMenus()
        {
            if (GUILayout.Button("Menu"))
            {
                OnClickMenu();
            }
        }

        protected override void OnGUITitleInfo()
        {
            base.OnGUITitleInfo();
        }

        protected virtual void OnGUIEditingRequirementsInfo()
        {
            this.CalculateBrotherNames();

            reorderableRequirementList.DoLayoutList();
            reorderableRequirementList.displayAdd = CheckDisplayAdd();
        }

        protected virtual void OnGUIEditingPropertiesInfo()
        {
            reorderablePropertiesList.DoLayoutList();
        }


        /// <summary>
        /// 检查要求是否已经使用了?
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckIsRequirementUsed(string name)
        {
            for (int i = 0; i < this._requirements.Count; ++i)
            {
                if (_requirements[i].nodeName == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否还允许继续添加
        /// </summary>
        private bool CheckDisplayAdd()
        {
            if (null == _parent)
                return false;

            List<NodeBase> children = _parent.Children;
            if (children.Count - 1 > brotherNames.Count)
                return true;

            foreach (NodeBase brother in children)
            {
                if (brother == this)
                    continue;

                if (!CheckIsRequirementUsed(brother.Name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 寻找第一个仍未被使用的要求名
        /// </summary>
        /// <returns></returns>
        private bool FindFirstUnusedRequirementName(out string name)
        {
            name = null;
            if (null == _parent)
                return false;

            List<NodeBase> children = _parent.Children;
            foreach (NodeBase brother in children)
            {
                if (brother == this)
                    continue;

                if (!CheckIsRequirementUsed(brother.Name))
                {
                    name = brother.Name;
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region GUI/Requirement
        private void OnDrawRequiementHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Requirements");

            rect.xMin += 100;
            rect.xMax = rect.xMin + 300;

            _requirementLogic = (ECheckLogic)EditorGUI.EnumPopup(rect, this._requirementLogic);
        }

        private void OnDrawRequiementElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;
            float width = rect.width;
            if (index < _requirements.Count)
            {
                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);

                Requirement item = _requirements[index];

                int currentSelectedIndex = this.GetBrotherIndex(item.nodeName);

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + rect.width / 3 - 2;
                int newSelectedIndex = EditorGUI.Popup(rect, currentSelectedIndex, this.brotherNames.ToArray());
                if (newSelectedIndex != currentSelectedIndex && newSelectedIndex >= 0)
                {
                    item.nodeName = this.brotherNames[newSelectedIndex];
                }

                rect.xMin = rect.xMax + 4;
                rect.width = width / 2 - 2;
                item.isRequirement = EditorGUI.Toggle(rect, "", item.isRequirement);

                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);
            }
            else
            {
                reorderableRequirementList.list.RemoveAt(index);
            }
        }

        private void OnAddRequiement()
        {
            Requirement requirement = new Requirement();
            bool ret = FindFirstUnusedRequirementName(out string name);
            if (ret)
                requirement.nodeName = name;

            _requirements.Add(requirement);
        }

        private void OnRemoveRequiement()
        {
            _requirements.RemoveAt(reorderableRequirementList.index);
        }
        #endregion

        #region GUI/Properties
        private void OnDrawPropertyHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Properties");
        }

        protected void OnDrawPropertyElement(Rect rect, int index, bool selected, bool focused)
        {
            Rect parentRect = rect;
            float width = rect.width;
            if (index < fieldInfos.Count)
            {
                GUIStyle labelStyle = new GUIStyle("minibutton")
                {
                    alignment = TextAnchor.MiddleLeft,
                };

                FieldInfo item = fieldInfos[index];

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.xMax = rect.xMin + width * 0.2f;
                EditorGUI.LabelField(rect, new GUIContent(item.Name, this.GetFieldDesc(item.Name)), labelStyle);

                Rect separationRect = Rect.MinMaxRect(rect.xMax + 2, rect.yMin, rect.xMax + 4, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);



                rect.xMin = rect.xMax + 6;
                rect.xMax = rect.xMin + width * 0.2f;

                // 如果是属性的话 则显示属性的参数类型
                if (item.FieldType.IsSubclassOf(typeof(PropertyBase)) && item.FieldType.IsGenericType)
                {
                    System.Type fieldType = item.FieldType;
                    System.Type[] args = fieldType.GenericTypeArguments;
                    if (null != args && args.Length > 0)
                    {
                        string genericType = args[0].ToString();
                        if (!string.IsNullOrEmpty(genericType))
                        {
                            if (genericType.Contains('.'))
                            {
                                genericType = genericType[(genericType.LastIndexOf('.') + 1)..];
                            }
                            EditorGUI.LabelField(rect, new GUIContent(genericType, fieldType.ToString()), labelStyle);
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(rect, new GUIContent(fieldType.ToString()), labelStyle);
                    }
                }
                else
                {
                    string fieldType = item.FieldType.ToString();
                    if (!string.IsNullOrEmpty(fieldType))
                    {
                        if (fieldType.Contains('.'))
                        {
                            fieldType = fieldType[(fieldType.IndexOf('.') + 1)..];
                        }
                        EditorGUI.LabelField(rect, new GUIContent(fieldType, item.FieldType.BaseType.ToString()), labelStyle);
                    }
                }

                separationRect = Rect.MinMaxRect(rect.xMax + 2, rect.yMin, rect.xMax + 4, rect.yMax);
                EditorGUI.DrawRect(separationRect, Logic.Utility.kSplitLineColor);

                OnDrawUnderlyingFieldValue(ref rect, parentRect, item);

                Rect lineRect = Rect.MinMaxRect(parentRect.xMin + 4, parentRect.yMax - 2, parentRect.xMax - 4, parentRect.yMax);
                EditorGUI.DrawRect(lineRect, Logic.Utility.kSplitLineColor);
            }
            else
            {
                reorderableRequirementList.list.RemoveAt(index);
            }
        }

        protected virtual void OnDrawUnderlyingFieldValue(ref Rect rect, Rect parentRect, FieldInfo field)
        {
            rect.xMin = rect.xMax + 6;
            rect.xMax = parentRect.xMax;

            if (field.FieldType == typeof(int))
            {
                int v = (int)field.GetValue(this);
                v = EditorGUI.IntField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(float))
            {
                float v = (float)field.GetValue(this);
                v = EditorGUI.FloatField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(bool))
            {
                bool v = (bool)field.GetValue(this);
                v = EditorGUI.Toggle(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(string))
            {
                string v = (string)field.GetValue(this);
                v = EditorGUI.TextField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(FixFactor))
            {
                FixVector2 v = (FixVector2)field.GetValue(this);
                Vector2Int vi = new Vector2Int(v.x, v.y);
                vi = EditorGUI.Vector2IntField(rect, new GUIContent(""), vi);
                v = new FixVector2(vi.x, vi.y);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(FixVector2))
            {
                FixVector2 v = (FixVector2)field.GetValue(this);
                Vector2Int vi = new Vector2Int(v.x, v.y);
                vi = EditorGUI.Vector2IntField(rect, new GUIContent(""), vi);
                v = new FixVector2(vi.x, vi.y);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(FixVector3))
            {
                FixVector3 v = (FixVector3)field.GetValue(this);
                Vector3Int vi = new Vector3Int(v.x, v.y, v.z);
                vi = EditorGUI.Vector3IntField(rect, new GUIContent(""), vi);
                v = new FixVector3(vi.x, vi.y, vi.z);
                field.SetValue(this, v);
            }
            else if (field.FieldType == typeof(Color))
            {
                Color v = (Color)field.GetValue(this);
                v = EditorGUI.ColorField(rect, v);
                field.SetValue(this, v);
            }
            else if (field.FieldType.IsEnum)
            {
                System.Enum v = (System.Enum)field.GetValue(this);
                v = EditorGUI.EnumPopup(rect, v);

                field.SetValue(this, v);
            }
            EditorGUI.EndChangeCheck();
        }

        #endregion

        #region Listen
        protected virtual void OnClickRemove()
        {
            this.SetParent(null);
            this.Recycle();
        }

        protected virtual void OnClickMenu()
        {
            bool isDisable;

            GenericMenu menu = new GenericMenu();
           
            menu.AddItem(new GUIContent("Remove"), false, (object userData) =>
            {
                this.OnClickRemove();
            }, null);

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Copy"), false, (object userData) =>
            {
                Utility.copyNodeBuffer = this.Clone() as NodeBase;
            }, null);
            isDisable = (Utility.copyNodeBuffer == null || Utility.copyNodeBuffer.GetType() != this.GetType());
            if (isDisable)
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }
            else
            {
                menu.AddItem(new GUIContent("Paste"), false, (object userData) =>
                {
                    Utility.copyNodeBuffer.CloneTo(this);
                    this.SetDirty();
                }, null);
            }

            menu.ShowAsContext();

        }
        #endregion
#endif
    }
}
