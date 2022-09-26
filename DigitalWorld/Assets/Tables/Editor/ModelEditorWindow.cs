using DigitalWorld.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Table.Editor
{
    internal class ModelEditorWindow : EditorWindow
    {
        #region Params
        private static ModelEditorWindow window = null;

        #endregion

        #region Window
        internal static ModelEditorWindow FocusWindow()
        {
            if (null != window)
            {
                window.Focus();
            }
            else
            {
                window = EditorWindow.CreateWindow<ModelEditorWindow>(typeof(TableEditorWindow), null);
                window.Show();
            }

            return window;
        }
        #endregion

        #region GUI
        private void OnGUI()
        {

        }
        #endregion
    }
}
