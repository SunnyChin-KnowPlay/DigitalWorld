using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicActionEditorWindow : LogicEffectEditorWindow
    {

        #region Params

        private EAction selectAction = 0;

        private List<EAction> typeEnums = new List<EAction>();

        private static readonly List<EAction> enumList = new List<EAction>();

        #endregion

        #region Common
        private int OnSortAction(EAction l, EAction r)
        {
            return string.Compare(l.ToString(), r.ToString());
        }

        protected override string GetTitle(EShowMode mode)
        {
            switch (mode)
            {
                case EShowMode.Add:
                    return "新增 动作";
                case EShowMode.Edit:
                    return "编辑 动作";
                default:
                    return "";
            }
        }

        public static EditorWindow GetWindow()
        {
            LogicActionEditorWindow window = EditorWindow.GetWindow<LogicActionEditorWindow>();
            return window;
        }
        #endregion

        #region GUI
        public override void Show(Logic.NodeBase parent)
        {
            base.Show(parent);

            this.selectAction = 0;
        }

        public override void Show(Logic.NodeBase parent, Logic.NodeBase node)
        {
            base.Show(parent, node);

            selectAction = (EAction)node.Id;
        }

        protected override void OnGUIBody()
        {
            typeEnums.Clear();
            typeIndexs.Clear();
            typeNames.Clear();

            int currentIndex = 0;
            int index = 0;

            enumList.Clear();
            foreach (EAction i in System.Enum.GetValues(typeof(EAction)))
            {
                enumList.Add(i);
            }
            enumList.Sort(OnSortAction);


            foreach (EAction i in enumList)
            {
                if (i == selectAction)
                {
                    typeEnums.Add(i);
                    typeIndexs.Add(index);
                    typeNames.Add(string.Format("{0} - {1}", i, Defined.GetActionDesc(i)));
                    currentIndex = index;
                    ++index;
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        string fil = filter.ToLower();

                        if (i.ToString().ToLower().Contains(fil) || Defined.GetActionDesc(i).ToLower().Contains(fil))
                        {
                            typeEnums.Add(i);
                            typeIndexs.Add(index);
                            typeNames.Add(string.Format("{0} - {1}", i, Defined.GetActionDesc(i)));
                            ++index;
                        }
                    }
                    else
                    {
                        typeEnums.Add(i);
                        typeIndexs.Add(index);
                        typeNames.Add(string.Format("{0} - {1}", i, Defined.GetActionDesc(i)));
                        ++index;
                    }
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("动作 类型");

            EAction old = this.selectAction;
            int oldIndex = currentIndex;
            currentIndex = EditorGUILayout.IntPopup(currentIndex, typeNames.ToArray(), typeIndexs.ToArray());
            EditorGUILayout.EndHorizontal();

            if (null == currentNode || oldIndex != currentIndex)
            {
                selectAction = typeEnums[currentIndex];
                if (selectAction != 0)
                {
                    currentNode = Logic.Utility.CreateNewAction(selectAction);
                }
                else
                {
                    currentNode = null;
                }
            }

            if (null != currentNode)
            {
                currentNode.OnGUIDetails();
            }
        }



        #endregion


    }
}
