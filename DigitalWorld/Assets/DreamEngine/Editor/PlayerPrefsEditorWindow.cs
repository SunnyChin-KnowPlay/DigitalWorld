using UnityEditor;
using UnityEngine;

namespace DreamEditor
{
    public class PlayerPrefsEditorWindow : EditorWindow
    {
        [MenuItem("Dream/PlayerPrefs/Clear PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
        }
    }

}


