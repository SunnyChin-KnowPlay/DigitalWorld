using DigitalWorld.Proto.Game;
using System.Collections;
using UnityEngine;


namespace DigitalWorld.Game
{

    public class Map : MonoBehaviour
    {
        public MapData mapData; // 存储地图数据的对象
        private const float gridSize = 1f; // 格子大小为1米*1米

        void Start()
        {
            StartCoroutine(InitializeMap());
        }

        IEnumerator InitializeMap()
        {
            // 初始化地图，根据MapData实例化格子
            for (int i = 0; i < mapData.width * mapData.height; i++)
            {
                GridData gridData = mapData.gridData[i];

                string name = string.Format("Grid_{0}", i);
                // 实例化预制件并设置属性
                GameObject grid = new GameObject(name);
                grid.transform.localPosition = gridData.position;
                grid.transform.localScale = Vector3.one;
                grid.transform.SetParent(transform);

                yield return new WaitForEndOfFrame();
            }
        }
    }

}
