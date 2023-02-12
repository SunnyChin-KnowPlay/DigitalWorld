using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    public class LogicEditorWindow : EditorWindow
    {
        #region MenuItems
        [MenuItem("Logic/Nodes", priority = 1)]
        private static void ShowItems()
        {
            EditorWindow.GetWindow<LogicItemsEditorWindow>("Nodes Editor").Show();
        }


        [MenuItem("Logic/Levels", priority = 12)]
        private static void ShowLevels()
        {
            EditorWindow.GetWindow<LogicLevelListEditorWindow>("Levels").Show();
        }

        [MenuItem("Logic/Settings", priority = 24)]
        public static void Setting()
        {
            GetWindow<LogicSettingEditorWindow>("Logic Settings").ShowUtility();
        }
        #endregion
    }
}
