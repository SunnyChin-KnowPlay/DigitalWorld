using DreamEngine.UI;
using UnityEditor;
using UnityEngine;

namespace DreamEditor.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WidgetPanel))]
    public class WidgetPanelEditor : Editor
    {
        private WidgetPanel panelTarget;

        private void OnEnable()
        {
            panelTarget = (WidgetPanel)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            panelTarget.animationFunction = (EPanelSwitchAnimationFunction)EditorGUILayout.EnumFlagsField("Animation Functions", panelTarget.animationFunction);

            // 保存上面Toggle设置值
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
