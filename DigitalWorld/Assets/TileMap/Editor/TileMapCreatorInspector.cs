using DigitalWorld.Proto.Game;
using DigitalWorld.Table;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.TileMap.Editor
{
    [CustomEditor(typeof(TileMapCreator))]
    public class TileMapCreatorInspector : UnityEditor.Editor
    {
        public TileMapCreator tileMapCreator;

        private const string tileGridPath = "Assets/Res/Tile/TileGround.prefab";
        private GameObject modelGridObj = null;

        public void OnEnable()
        {
            if (target == null) return;
            tileMapCreator = (TileMapCreator)target;

            modelGridObj = AssetDatabase.LoadAssetAtPath<GameObject>(tileGridPath) as GameObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (null != tileMapCreator)
            {
                if (GUILayout.Button(new GUIContent("创建地基")))
                {
                    CreateMapGround();
                }

                if (GUILayout.Button(new GUIContent("预创建地图")))
                {
                    CreateMapFromTable();
                }
            }
        }

        #region Common
        private void CreateMapGround()
        {
            if (null == tileMapCreator)
                return;

            if (tileMapCreator.width <= 0 || tileMapCreator.height <= 0)
            {
                UnityEngine.Debug.LogError("tilemap size err!");
                return;
            }

            if (null == modelGridObj)
            {
                UnityEngine.Debug.LogError("model grid not found!");
                return;
            }

            GameObject go = new GameObject("NewTileMap");
            TileMapControl control = go.AddComponent<TileMapControl>();

            int size = tileMapCreator.width * tileMapCreator.height;
            control.mapSize = new Dream.FixMath.FixVector2(tileMapCreator.width, tileMapCreator.height);
            control.gridSize = tileMapCreator.gridSize;
            control.grids = new TileGrid[size];
            for (int i = 0; i < size; ++i)
            {
                GameObject tileGo = GameObject.Instantiate<GameObject>(modelGridObj);
                tileGo.isStatic = true;
                tileGo.transform.SetParent(control.transform, false);
                control.grids[i] = tileGo.AddComponent<TileGrid>();
            }

            go.isStatic = true;

            control.CalculateGrids();
        }

        private void CreateMapFromTable()
        {
            TableManager tm = TableManager.instance;
            tm.Decode();

            Dictionary<int, MapInfo> maps = tm.MapTable.Infos;
            foreach (KeyValuePair<int, MapInfo> kvp in maps)
            {
                MapInfo info = kvp.Value;
                MapData data = new MapData();
                data.mapId = info.id;
                data.level = 1;

                WriteMap(data, data.mapId.ToString());
            }

            AssetDatabase.Refresh();
        }

        private void WriteMap(MapData data, string name)
        {
            if (null != data)
            {
                int size = data.CalculateSize();
                byte[] buffer = new byte[size];
                data.Encode(buffer, 0);

                string fileName = string.Format("{0}.bytes", name);
                string path = Path.Combine(TileMapControl.defaultMapDataPath, fileName);
                string fullPath = Path.Combine(Application.dataPath, path);

                if (File.Exists(fullPath))
                {
                    return;
                }

                string directoryPath = Path.GetDirectoryName(fullPath);
                if (string.IsNullOrEmpty(directoryPath))
                    return;

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using FileStream fs = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(buffer);
            }
        }
        #endregion
    }
}
