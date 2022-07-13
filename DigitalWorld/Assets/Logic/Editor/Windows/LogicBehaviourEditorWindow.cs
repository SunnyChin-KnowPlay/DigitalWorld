using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class LogicBehaviourEditorWindow : EditorWindow
    {
        #region Params
        private Vector2 fileListScrollViewPos = Vector2.zero;


        #endregion

        #region GUI
        private void OnGUI()
        {
            fileListScrollViewPos = EditorGUILayout.BeginScrollView(fileListScrollViewPos);


           
            EditorGUILayout.EndScrollView();
        }
        #endregion
    }
}
