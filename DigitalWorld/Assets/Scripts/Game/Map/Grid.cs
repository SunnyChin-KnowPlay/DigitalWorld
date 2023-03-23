using Dream.Core;
using UnityEngine;

namespace DigitalWorld.Game
{
    using UnityEngine;

    public class Grid : MonoBehaviour
    {
        public MapData mapData; // 存储地图数据的对象

        void OnDrawGizmos()
        {
            // 绘制网格
            for (int i = 0; i < mapData.width * mapData.height; i++)
            {
                Vector3 position = mapData.gridData[i].position;
                int x = i % mapData.width;
                int y = i / mapData.width;
                Gizmos.color = (x + y) % 2 == 0 ? new Color(1, 1, 1, 0.5f) : new Color(0.8f, 0.8f, 0.8f, 0.5f);
                Gizmos.DrawCube(position, Vector3.one);
            }
        }
    }

}
