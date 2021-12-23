using UnityEditor;
using UnityEngine;

namespace DigitalWorld.TileMap.Editor
{
    [CustomEditor(typeof(TileMapControl))]
    public class TileMapControlInspector : UnityEditor.Editor
    {
        public TileMapControl tileMapControl;

        private void OnEnable()
        {
            if (target == null) return;
            tileMapControl = (TileMapControl)target;

            tileMapControl.StopEdit();

            SceneView.duringSceneGui += OnDuringScene;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnDuringScene;
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

                if (GUILayout.Button(new GUIContent("清空地图")))
                {
                    tileMapControl.Clear();
                }

                if (tileMapControl.IsEditing)
                {
                    if (GUILayout.Button(new GUIContent("停止编辑")))
                    {
                        this.OnStopEdit();
                    }
                }
                else
                {
                    if (GUILayout.Button(new GUIContent("开始编辑")))
                    {
                        this.OnStartEdit();
                    }
                }
            }
        }

        #region Callback
        private void OnStartEdit()
        {
            tileMapControl.StartEdit();
        }

        private void OnStopEdit()
        {
            tileMapControl.StopEdit();
        }

        private void OnDuringScene(SceneView sceneView)
        {
            if (EditorWindow.mouseOverWindow == sceneView && tileMapControl.IsEditing)
            {
                this.UpdateInScene();
            }
        }
        #endregion

        #region Update
        private void UpdateInScene()
        {
           
        }
        #endregion
    }
}
