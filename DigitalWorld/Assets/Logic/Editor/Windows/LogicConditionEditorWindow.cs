using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicConditionEditorWindow : LogicEffectEditorWindow
    {

        #region Params

        private ECondition selectCondition = 0;

        private readonly List<ECondition> typeEnums = new List<ECondition>();

        private static readonly List<ECondition> enumList = new List<ECondition>();

        #endregion

        #region Common
        private int OnSortCondition(ECondition l, ECondition r)
        {
            return string.Compare(l.ToString(), r.ToString());
        }

        protected override string GetTitle(EShowMode mode)
        {
            return mode switch
            {
                EShowMode.Add => "Creating Condition",
                EShowMode.Edit => "Editing Condition",
                _ => "",
            };
        }

        public static EditorWindow GetWindow()
        {
            LogicConditionEditorWindow window = EditorWindow.GetWindow<LogicConditionEditorWindow>();
            return window;
        }
        #endregion

        #region GUI
        public override void Show(Logic.NodeBase parent)
        {
            base.Show(parent);

            this.selectCondition = 0;
        }

        public override void Show(Logic.NodeBase parent, Logic.NodeBase node)
        {
            base.Show(parent, node);

            selectCondition = (ECondition)node.Id;
        }

        protected override void OnGUIBody()
        {
            typeEnums.Clear();
            typeIndexs.Clear();
            typeNames.Clear();

            int currentIndex = 0;
            int index = 0;

            enumList.Clear();
            foreach (ECondition i in System.Enum.GetValues(typeof(ECondition)))
            {
                enumList.Add(i);
            }
            enumList.Sort(OnSortCondition);


            foreach (ECondition i in enumList)
            {
                if (i == selectCondition)
                {
                    typeEnums.Add(i);
                    typeIndexs.Add(index);
                    typeNames.Add(string.Format("{0} - {1}", i, Defined.GetConditionDesc(i)));
                    currentIndex = index;
                    ++index;
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        string fil = filter.ToLower();

                        if (i.ToString().ToLower().Contains(fil) || Defined.GetConditionDesc(i).ToLower().Contains(fil))
                        {
                            typeEnums.Add(i);
                            typeIndexs.Add(index);
                            typeNames.Add(string.Format("{0} - {1}", i, Defined.GetConditionDesc(i)));
                            ++index;
                        }
                    }
                    else
                    {
                        typeEnums.Add(i);
                        typeIndexs.Add(index);
                        typeNames.Add(string.Format("{0} - {1}", i, Defined.GetConditionDesc(i)));
                        ++index;
                    }
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Condition Type");

            ECondition old = this.selectCondition;
            int oldIndex = currentIndex;
            currentIndex = EditorGUILayout.IntPopup(currentIndex, typeNames.ToArray(), typeIndexs.ToArray());
            EditorGUILayout.EndHorizontal();

            if (null == currentNode || oldIndex != currentIndex)
            {
                selectCondition = typeEnums[currentIndex];
                if (selectCondition != 0)
                {
                    currentNode = Logic.Utility.CreateNewCondition(selectCondition);
                }
                else
                {
                    currentNode = null;
                }
            }

            if (null != currentNode)
            {
                currentNode.OnGUIExtEditing();
            }
        }



        #endregion


    }
}
