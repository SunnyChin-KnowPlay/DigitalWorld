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
        protected Transform trans;

        public float distance = 15;

        public Vector2 distanceClamp = new Vector2(5, 25);
        public Vector2 heightAngleClamp = new Vector2(15, 75);

        public const float mouseScrollWheelSpeed = 1;

        /// <summary>
        /// 相机和目标的偏移量
        /// </summary>
        private Vector3 dir;
        /// <summary>
        /// 标准方向，就是直后方
        /// </summary>
        private Vector3 standardDir;

        /// <summary>
        /// 最后一次的目标位置
        /// </summary>
        private Vector3 lastedTargetPosition;

        private Vector3 oldMousePosition;
        public const float mouseMoveSpeed = 90;

        private float standardDirSqrMagnitude = 0;
        private const float standardDirSqrMagnitudeLimit = 1;

        private void Start()
        {
            trans = this.transform;
            dir = new Vector3(0, 10, -10) * distance;
        }

        void Update()
        {
            this.distance = Mathf.Clamp(this.distance + Input.GetAxis("Mouse ScrollWheel") * mouseScrollWheelSpeed, distanceClamp.x, distanceClamp.y);

            float mag = (target.position - lastedTargetPosition).sqrMagnitude;
            lastedTargetPosition = target.position;


            standardDirSqrMagnitude += mag;

            standardDir = target.position - target.forward * distance - target.up * distance;

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                oldMousePosition = Input.mousePosition;
            }

            if (null != target)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                    this.oldMousePosition = Input.mousePosition;

                    trans.RotateAround(target.position, target.right, mouseMoveSpeed * Time.deltaTime * deltaPosition.y);
                    trans.RotateAround(target.position, Vector3.up, mouseMoveSpeed * Time.deltaTime * deltaPosition.x);
                    dir = trans.position - target.position;

                    standardDirSqrMagnitude = 0;
                }

                UnityEngine.Debug.Log(string.Format("dir:{0}\tstandardDir:{1}", dir, standardDir));
                UnityEngine.Debug.Log(string.Format("mag:{0}\trate:{1}", mag, standardDirSqrMagnitude / standardDirSqrMagnitudeLimit));
                Vector3 finalDir = Vector3.Lerp(dir, standardDir, standardDirSqrMagnitude / standardDirSqrMagnitudeLimit);
                finalDir = dir;
                trans.position = target.position + finalDir.normalized * distance;


                trans.LookAt(target.position);
            }
        }
    }
}

