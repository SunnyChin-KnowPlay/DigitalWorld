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
        [MenuItem("Logic/生成代码", priority = 12)]
        private static void GenerateNodes()
        {
            NodeController.instance.GenerateNodes();
        }

        [MenuItem("Logic/节点编辑器", priority = 13)]
        private static void ShowItems()
        {
            EditorWindow.GetWindow<LogicItemsEditorWindow>("节点编辑器").Show();
        }
    }
}
