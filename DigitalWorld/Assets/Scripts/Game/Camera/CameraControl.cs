using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 战斗相机控制器
    /// </summary>
    public class CameraControl : MonoBehaviour
    {
        public Transform target;
        public float distanceUp = 10f;//相机与目标的竖直高度参数
        public float distanceAway = 10f;//相机与目标的水平距离参数
        public float smooth = 2f;//位置平滑移动插值参数值
        public float camDepthSmooth = 20f;

        private Transform trans;

        private void Start()
        {
            trans = this.transform;
        }

        void Update()
        {
            // 鼠标轴控制相机的远近
            if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) || Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80)
            {
                Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
            }
        }

        void LateUpdate()
        {
            if (null != target)
            {
                //计算出相机的位置
                Vector3 disPos = target.position + Vector3.up * distanceUp - target.forward * distanceAway;

                trans.position = Vector3.Lerp(trans.position, disPos, Time.deltaTime * smooth);
                //相机的角度
                trans.LookAt(target.position);
            }


        }
    }

}

