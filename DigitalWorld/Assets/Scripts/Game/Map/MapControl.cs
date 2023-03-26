using DigitalWorld.Asset;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace DigitalWorld.Game
{

    public class MapControl : MonoBehaviour
    {
        #region Constant
        private const string gridObjectPath = "Assets/Res/Grids/Grid.prefab";
        #endregion

        #region Params
        /// <summary>
        /// 存储地图数据的对象
        /// </summary>
        private MapData mapData;
        /// <summary>
        /// 网格线的材质
        /// </summary>
        private Material gridLineMaterial;
        
        /// <summary>
        /// 格子队列
        /// </summary>
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

        #region Setup
        public void Setup(MapData data)
        {
            this.mapData = data;
        }

        private void CreateGridLines()
        {
            GameObject gridLines = new GameObject("GridLines");
            gridLines.transform.SetParent(transform);
            gridLines.transform.localPosition = Vector3.zero;

            MeshFilter meshFilter = gridLines.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = gridLines.AddComponent<MeshRenderer>();

            meshRenderer.sharedMaterial = gridLineMaterial;

            Mesh mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();

            int halfWidth = mapData.width / 2;
            int halfHeight = mapData.height / 2;

            for (int x = 0; x <= mapData.width; x++)
            {
                vertices.Add(new Vector3(x - halfWidth, 0, -halfHeight));
                vertices.Add(new Vector3(x - halfWidth, 0, mapData.height - halfHeight));

                indices.Add(2 * x);
                indices.Add(2 * x + 1);
            }

            for (int z = 0; z <= mapData.height; z++)
            {
                vertices.Add(new Vector3(-halfWidth, 0, z - halfHeight));
                vertices.Add(new Vector3(mapData.width - halfWidth, 0, z - halfHeight));

                indices.Add(2 * (mapData.width + 1) + 2 * z);
                indices.Add(2 * (mapData.width + 1) + 2 * z + 1);
            }

            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }

        private IEnumerator InitializeMap()
        {
            grids.Clear();

            // 初始化地图，根据MapData实例化格子
            for (int y = 0; y < mapData.height; y++)
            {
                for (int x = 0; x < mapData.width; x++)
                {
                    int i = y * mapData.width + x;
                    GridData gridData = mapData.gridData[i];

                    string name = string.Format("Grid_{0}_{1}", y, x); // 修改后的名称，包含行和列信息
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
                }
                yield return new WaitForEndOfFrame();
            }
        }
        #endregion

        #region Logic
        private void Awake()
        {
            GameObject gridObject = AssetManager.LoadAsset<GameObject>(gridObjectPath);
            if (null != gridObject)
            {
                MeshRenderer mr = gridObject.GetComponent<MeshRenderer>();
                this.gridLineMaterial = mr.sharedMaterial;
            }
        }

        private void Start()
        {
            CreateGridLines();
            StartCoroutine(InitializeMap());
        }

        
        #endregion

       
    }

}
