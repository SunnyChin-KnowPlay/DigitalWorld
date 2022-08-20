using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    public class LogicEditorWindow : EditorWindow
    {
        [MenuItem("Logic/Editor/Nodes", priority = 1)]
        private static void ShowItems()
        {
            EditorWindow.GetWindow<LogicItemsEditorWindow>("Nodes Editor").Show();
        }
    }
}
