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

        [MenuItem("Logic/Editor", priority = 1)]
        [MenuItem("Logic/Editor/Nodes", priority = 1)]
        private static void ShowItems()
        {
            EditorWindow.GetWindow<LogicItemsEditorWindow>("Nodes Editor").Show();
        }

        [MenuItem("Logic/Editor/Behaviours", priority = 2)]
        private static void ShowBehaviours()
        {
            EditorWindow.GetWindow<LogicBehaviourEditorWindow>("Behaviours Editor").Show();
        }

        [MenuItem("Logic/Generater", priority = 20)]
        [MenuItem("Logic/Generater/Codes", priority = 1)]
        private static void GenerateNodesCode()
        {
            NodeController.instance.GenerateNodesCode();
        }
    }
}
