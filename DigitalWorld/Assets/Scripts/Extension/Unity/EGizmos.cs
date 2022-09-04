using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DigitalWorld.Extension.Unity
{
    public static class EGizmos
    {
        // theta越小，线段绘制数越多，圆环越平滑。
        public static void GizmosDrawCircle(this MonoBehaviour monoBehaviour, Transform transform, float radius, float theta, Color color)
        {
            var matrix = Gizmos.matrix; //保存原本的矩阵信息
            Gizmos.matrix = transform.localToWorldMatrix; //应用目标trans矩阵信息
            Gizmos.color = color; //设置颜色
            Vector3 beginPoint = new Vector3(radius, 0, 0); //定义起始点
            Vector3 firstPoint = new Vector3(radius, 0, 0); //定义起始点
            for (float t = 0; t < 2 * Mathf.PI; t += theta) //循环线段绘制
            {
                float x = radius * Mathf.Cos(t); //计算cos
                float z = radius * Mathf.Sin(t); //计算sin
                Vector3 endPoint = new Vector3(x, 0, z); //确定圆环采样点
                Gizmos.DrawLine(beginPoint, endPoint); //绘制线段
                beginPoint = endPoint; //迭代赋值
            }
            Gizmos.DrawLine(firstPoint, beginPoint); //绘制最后一段
            Gizmos.matrix = matrix; //还原Gizmos矩阵信息
        }
    }
}
