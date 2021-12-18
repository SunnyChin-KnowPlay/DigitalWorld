using UnityEditor;
using UnityEngine;

namespace DigitalWorld.TileMap.Editor
{
    [CustomEditor(typeof(TileMapControl))]
    public class TileMapControlInspector : UnityEditor.Editor
    {
        public TileMapControl tileMapControl;
      
        public void OnEnable()
        {
            if (target == null) return;
            tileMapControl = (TileMapControl)target;

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (null != tileMapControl)
            {
                if (GUILayout.Button(new GUIContent("配置地图")))
                {
                    tileMapControl.CalculateGrids();
                }
            }
        }
    }
}
