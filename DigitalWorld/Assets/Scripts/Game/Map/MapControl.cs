using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DigitalWorld.Game
{

    public class MapControl : MonoBehaviour
    {
        #region Params
        public MapData mapData; // 存储地图数据的对象

        private readonly List<GridControl> grids = new List<GridControl>();
        #endregion

        #region Creator
        public static MapControl Create(MapData data)
        {
            GameObject go = new GameObject("Map");
            MapControl map = go.AddComponent<MapControl>();
            map.Setup(data);
            return map;
        }
        #endregion

        #region Logic
        private void Start()
        {
            StartCoroutine(InitializeMap());
        }

        private IEnumerator InitializeMap()
        {
            grids.Clear();

            // 初始化地图，根据MapData实例化格子
            for (int i = 0; i < mapData.width * mapData.height; i++)
            {
                GridData gridData = mapData.gridData[i];

                string name = string.Format("Grid_{0}", i);
                // 实例化预制件并设置属性
                GameObject gridObject = new GameObject(name);
                gridObject.transform.localPosition = gridData.position;
                gridObject.transform.localScale = Vector3.one;
                gridObject.transform.SetParent(transform);

                if (!gridObject.TryGetComponent<GridControl>(out GridControl grid))
                {
                    grid = gridObject.AddComponent<GridControl>();
                }
                if (null != grid)
                {
                    grid.gridData = gridData;
                    grids.Add(grid);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

        #region Setup
        public void Setup(MapData data)
        {
            this.mapData = data;
        }
        #endregion
    }

}
