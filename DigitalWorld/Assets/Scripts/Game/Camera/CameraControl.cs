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
        /// 鼠标左键的dir
        /// </summary>
        private Vector3 leftDir;
        /// <summary>
        /// 鼠标右键的相机和目标的偏移量
        /// </summary>
        private Vector3 rightDir;

        /// <summary>
        /// 最后一次的目标位置
        /// </summary>
        private Vector3 lastedTargetPosition;

        private Vector3 oldMousePosition;
        public float mouseMoveSpeed = 90;


        private float standardDirSqrMagnitude = 0;
        private const float standardDirSqrMagnitudeLimit = 1;

        private void Start()
        {
            trans = this.transform;
            leftDir = new Vector3(0, 10, -10) * distance;
            rightDir = new Vector3(0, 10, -10) * distance;
        }

        void Update()
        {
            this.distance = Mathf.Clamp(this.distance + Input.GetAxis("Mouse ScrollWheel") * mouseScrollWheelSpeed, distanceClamp.x, distanceClamp.y);

            float mag = (target.position - lastedTargetPosition).sqrMagnitude;
            lastedTargetPosition = target.position;

            standardDirSqrMagnitude += mag;

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                oldMousePosition = Input.mousePosition;
            }

            if (null != target)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                    this.oldMousePosition = Input.mousePosition;

                    trans.RotateAround(target.position, target.right, mouseMoveSpeed * Time.deltaTime * deltaPosition.y);
                    trans.RotateAround(target.position, Vector3.up, this.mouseMoveSpeed * Time.deltaTime * deltaPosition.x);

                    leftDir = trans.position - target.position;

                    float cos = Vector3.Dot(Vector3.up, leftDir) / Vector3.Magnitude(leftDir);
                    if (cos <= 0 || cos >= 0.99f)
                    {
                        trans.RotateAround(target.position, target.right, -mouseMoveSpeed * Time.deltaTime * deltaPosition.y);
                        leftDir = trans.position - target.position;
                    }

                    standardDirSqrMagnitude = 0;
                }

                if (Input.GetMouseButton(1))
                {
                    Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                    this.oldMousePosition = Input.mousePosition;

                    trans.RotateAround(target.position, target.right, mouseMoveSpeed * Time.deltaTime * deltaPosition.y);
                    trans.RotateAround(target.position, Vector3.up, this.mouseMoveSpeed * Time.deltaTime * deltaPosition.x);

                    leftDir = rightDir = trans.position - target.position;

                    float cos = Vector3.Dot(Vector3.up, rightDir) / Vector3.Magnitude(rightDir);
                    if (cos <= 0 || cos >= 0.99f)
                    {
                        trans.RotateAround(target.position, target.right, -mouseMoveSpeed * Time.deltaTime * deltaPosition.y);
                        leftDir = rightDir = trans.position - target.position;
                    }
                }


                Vector3 finalDir = Vector3.Lerp(leftDir, rightDir, standardDirSqrMagnitude / standardDirSqrMagnitudeLimit);
                trans.position = target.position + finalDir.normalized * distance;

                trans.LookAt(target.position);
            }
        }
    }
}

