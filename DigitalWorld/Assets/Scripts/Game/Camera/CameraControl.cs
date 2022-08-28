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

        private Vector3 offset;
        private bool rightButtonPressed = false;

        protected Vector3 lookAtRotation;
        public float mouseTurnedSpeed;

        protected Transform trans;

        //记录上一frame的旋转叫
        private Vector3 lastFrameTargetRoation;

        private void Start()
        {
            trans = this.transform;
            offset = new Vector3(0, 10, -10);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                rightButtonPressed = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                rightButtonPressed = false;
            }
        }

        void LateUpdate()
        {
            if (null != target)
            {

                trans.position = target.position + offset;
                trans.LookAt(target.position);

                if (rightButtonPressed)
                {
                    //获取鼠标旋转的度数 横轴
                    float rotationAmount = Input.GetAxis("Mouse X") * mouseTurnedSpeed * Time.deltaTime;
                    if (rotationAmount != 0)
                    {
                        int x = 1;
                    }
                    //最终的旋转读书
                    trans.RotateAround(target.position, Vector3.up, rotationAmount * 360);
                    //人物也旋转 保证镜头始终对着人物背面
                    target.RotateAround(target.position, Vector3.up, rotationAmount * 360);

                    ////纵轴
                    //float rotationAmountY = Input.GetAxis("Mouse Y") * mouseTurnedSpeed * Time.deltaTime;
                    //Vector3 yCenter = new Vector3(-offset.z / offset.x, target.position.y, 1);
                    //trans.RotateAround(target.position, yCenter, rotationAmountY * 360);
                }
                else
                {
                    trans.RotateAround(target.position, Vector3.up, target.rotation.eulerAngles.y - lastFrameTargetRoation.y);

                }

                lastFrameTargetRoation = target.rotation.eulerAngles;

            }



        }
    }

}

