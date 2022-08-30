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
        public float distance = 15;

        public Vector2 distanceClamp = new Vector2(5, 25);
        public Vector2 heightAngleClamp = new Vector2(15, 75);

        public const float mouseScrollWheelSpeed = 1;

        /// <summary>
        /// 相机和目标的偏移量
        /// </summary>
        private Vector3 offset;
        private Vector3 oldMousePosition;
        public const float mouseMoveSpeed = 90;
        /// <summary>
        /// 是否按住了左键
        /// </summary>
        private bool isPressingLeftMouse = false;

        /// <summary>
        /// 左键的旋转欧拉角
        /// </summary>
        private Vector3 mouseEulers;

        protected Vector3 lookAtRotation;
        public float mouseTurnedSpeed;

        protected Transform trans;


        private void Start()
        {
            trans = this.transform;
            mouseEulers = new Vector3(90, 45, 0);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                oldMousePosition = Input.mousePosition;
            }

            this.distance = Mathf.Clamp(this.distance + Input.GetAxis("Mouse ScrollWheel") * mouseScrollWheelSpeed, distanceClamp.x, distanceClamp.y);

            if (Input.GetMouseButtonDown(0))
            {
                isPressingLeftMouse = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isPressingLeftMouse = false;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                this.oldMousePosition = Input.mousePosition;
                mouseEulers += mouseMoveSpeed * Time.deltaTime * deltaPosition;

                //this.mouseEulers.x += deltaPosition.x * mouseMoveSpeed * Time.deltaTime;
                this.mouseEulers.y = Mathf.Clamp(this.mouseEulers.y, this.heightAngleClamp.x, this.heightAngleClamp.y);

            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                this.oldMousePosition = Input.mousePosition;
                mouseEulers += mouseMoveSpeed * Time.deltaTime * deltaPosition;

                this.mouseEulers.y = Mathf.Clamp(this.mouseEulers.y, this.heightAngleClamp.x, this.heightAngleClamp.y);
                //UnityEngine.Debug.Log(heightAngleOfView);
            }

            if (null != target)
            {
                offset = Vector3.zero;

                if (isPressingLeftMouse) // 如果是左键过程中 则是直接移动镜头的 
                {
                    offset += distance * Mathf.Tan(mouseEulers.x * Mathf.Deg2Rad) * target.right;
                    offset += distance * Mathf.Sin(mouseEulers.y * Mathf.Deg2Rad) * target.up;
                    offset += distance * Mathf.Cos(mouseEulers.y * Mathf.Deg2Rad) * -target.forward;
                }
                else
                {
                    offset += distance * Mathf.Sin(mouseEulers.y * Mathf.Deg2Rad) * target.up;
                    offset += distance * Mathf.Cos(mouseEulers.y * Mathf.Deg2Rad) * -target.forward;
                }

                trans.position = target.position + offset;
                trans.LookAt(target.position);
            }
        }


    }

}

