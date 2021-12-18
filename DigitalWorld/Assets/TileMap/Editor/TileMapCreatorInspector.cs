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
                if (GUILayout.Button(new GUIContent("创建地图")))
                {
                    CreateMap();
                }
            }
        }

        #region Common
        private void CreateMap()
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
        #endregion
    }
}
