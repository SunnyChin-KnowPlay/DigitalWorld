using DigitalWorld.Logic;
using DigitalWorld.Proto.Game;
using System.IO;
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


        }

        private void OnDisable()
        {

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
                TextAsset mapTA = EditorGUILayout.ObjectField(tileMapControl.CurrentMapAsset, typeof(TextAsset), false) as TextAsset;
                if (mapTA != tileMapControl.CurrentMapAsset)
                {
                    this.tileMapControl.OpenMap(mapTA);
                    this.OnStartEdit();
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button(new GUIContent("配置地块")))
                {
                    tileMapControl.CalculateGrids();
                    EditorUtility.SetDirty(tileMapControl.gameObject);
                }

                EditorGUI.BeginDisabledGroup(tileMapControl.CurrentMapAsset == null);

                if (tileMapControl.IsEditing)
                {
                    if (GUILayout.Button(new GUIContent("保存地图")))
                    {
                        this.OnSave();
                    }

                    if (GUILayout.Button(new GUIContent("停止编辑")))
                    {
                        this.OnStopEdit();
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

                EditorGUI.EndDisabledGroup();
            }
        }
        #endregion

        #region Callback
        private void OnStartEdit()
        {
            SceneView.duringSceneGui += OnDuringScene;

            tileMapControl.StartEdit();
        }

        private void OnStopEdit()
        {
            tileMapControl.StopEdit();

            SceneView.duringSceneGui -= OnDuringScene;
        }

        private void OnDuringScene(SceneView sceneView)
        {
            if (EditorWindow.mouseOverWindow == sceneView && tileMapControl.IsEditing)
            {
                this.UpdateInScene();
            }
        }

        private void OnSave()
        {
            MapData data = tileMapControl.SaveMap();
            if (null != data)
            {
                int size = data.CalculateSize();
                byte[] buffer = new byte[size];
                data.Encode(buffer, 0);

                string fileName = string.Format("{0}.bytes", data.mapId);
                string path = Path.Combine(TileMapControl.defaultMapDataPath, fileName);
                string fullPath = Path.Combine(Application.dataPath, path);

                string directoryPath = Path.GetDirectoryName(fullPath);
                if (string.IsNullOrEmpty(directoryPath))
                    return;

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using FileStream fs = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(buffer);
                fs.Flush();
                fs.Close();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        #endregion

        #region Update
        private void UpdateInScene()
        {
            Event e = Event.current;

            if (e != null)
            {
                UpdateBuild(e);
                UpdateMove(e);
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

                            TileGrid grid = hit.collider.GetComponent<TileGrid>();
                            if (null != grid)
                            {
                                GameObject go = TileMapControl.currentEditTileGo;
                                if (null != go)
                                {
                                    GameObject currentGo = GameObject.Instantiate(go);
                                    ControlTile tile = currentGo.GetComponent<ControlTile>();
                                    if (null != tile)
                                    {
                                        this.tileMapControl.SetTile(tile, grid.Index);
                                    }
                                }
                                else
                                {
                                    this.tileMapControl.SetTile(null, grid.Index);
                                }
                            }

                            e.Use();
                            break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
