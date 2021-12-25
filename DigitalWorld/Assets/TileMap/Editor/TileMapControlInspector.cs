using DigitalWorld.Logic;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.TileMap.Editor
{
    [CustomEditor(typeof(TileMapControl))]
    public class TileMapControlInspector : UnityEditor.Editor
    {
        public TileMapControl tileMapControl;

        private SceneView sceneView;

        #region Mono
        private void OnEnable()
        {
            if (target == null) return;

            sceneView = (SceneView)EditorWindow.GetWindow(typeof(SceneView));

            tileMapControl = (TileMapControl)target;

            SceneView.duringSceneGui += OnDuringScene;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnDuringScene;
        }
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (null != tileMapControl)
            {
                // 选择地图文件
                EditorGUI.BeginDisabledGroup(tileMapControl.IsEditing);

                EditorGUI.EndDisabledGroup();

                if (tileMapControl.IsEditing)
                {
                   

                    if (GUILayout.Button(new GUIContent("停止编辑")))
                    {
                        this.OnStopEdit();
                    }

                    if (GUILayout.Button(new GUIContent("配置地图")))
                    {
                        tileMapControl.CalculateGrids();
                    }

                    if (GUILayout.Button(new GUIContent("清空地图")))
                    {
                        tileMapControl.Clear();
                    }

                    if (GUILayout.Button(new GUIContent("重置地块")))
                    {
                        tileMapControl.ResetGrids();
                    }

                    GameObject go = this.tileMapControl.CurrentSelectedTileGo;
                    go = EditorGUILayout.ObjectField(new GUIContent("当前地块"), go, typeof(GameObject), false) as GameObject;
                    if (go != this.tileMapControl.CurrentSelectedTileGo)
                    {
                        this.tileMapControl.SelectTileGo(go);
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
        #endregion

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
            Event e = Event.current;

            if (e != null)
            {
                GameObject go = TileMapControl.currentEditTileGo;
                if (null != go)
                {
                    UpdateBuild(e);
                    UpdateMove(e);
                }
            }
        }

        private void UpdateMove(Event e)
        {

            // 拖动或移动
            if (e.type == EventType.MouseDrag || e.type == EventType.MouseMove)
            {
                GameObject go = TileMapControl.currentEditTileGo;
                if (null != go)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                        {
                            Transform t = go.GetComponent<Transform>();
                            Transform groundTrans = hit.collider.transform;

                            t.position = groundTrans.position + Vector3.up;
                            t.localScale = groundTrans.localScale;
                        }

                    }
                }
            }
        }

        private void UpdateBuild(Event e)
        {
            // 按住ctrl抬起左键时才算做构件
            if (e.control && e.button == 0 && (e.type == EventType.MouseUp || e.type == EventType.Used))
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                RaycastHit[] hits = Physics.RaycastAll(ray);

                if (hits != null && hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; ++i)
                    {
                        RaycastHit hit = hits[i];
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                        {
                            GameObject go = TileMapControl.currentEditTileGo;
                            TileGrid grid = hit.collider.GetComponent<TileGrid>();
                            if (null != grid)
                            {
                                GameObject currentGo = GameObject.Instantiate(go);
                                ControlTile tile = currentGo.GetComponent<ControlTile>();
                                if (null != tile)
                                {
                                    grid.SetTile(tile);
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
