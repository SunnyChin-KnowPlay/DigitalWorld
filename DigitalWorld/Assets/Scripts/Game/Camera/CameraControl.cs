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
        #region Params
        public Transform target;
        protected Transform trans;

        public float distance = 15;

        public Vector2 distanceClamp = new Vector2(5, 25);
        public Vector2 verticalAngleClamp = new Vector2(0, 89);

        public const float mouseScrollWheelSpeed = 5;


        /// <summary>
        /// 鼠标右键的相机和目标的偏移量
        /// </summary>
        private Vector3 inputAngles;

        /// <summary>
        /// 最后一次的目标位置
        /// </summary>
        private Vector3 lastedTargetPosition;

        private Vector3 oldMousePosition;
        public float mouseMoveSpeed = 90;

        private float standardDirSqrMagnitude = 0;
        private const float standardDirSqrMagnitudeLimit = 1;
        #endregion

        #region Common
        private Quaternion GetStandardHorizontalRotation()
        {
            return Quaternion.Euler(0, target.rotation.eulerAngles.y, 0);
        }

        private float ClampAngle(float angle, float min, float max)
        {
            return Mathf.Clamp(angle, min, max);
        }
        #endregion

        private void Start()
        {
            trans = this.transform;
            inputAngles = new Vector3(0, 1, 1) * distance;
        }

        void Update()
        {
            float axis = Input.GetAxis("Mouse ScrollWheel");
            this.distance = Mathf.Clamp(this.distance + axis * mouseScrollWheelSpeed, distanceClamp.x, distanceClamp.y);
        }

        private void LateUpdate()
        {
            if (null != target)
            {
                float mag = (target.position - lastedTargetPosition).sqrMagnitude;
                lastedTargetPosition = target.position;

                standardDirSqrMagnitude += mag * 10f;

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    oldMousePosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                    this.oldMousePosition = Input.mousePosition;

                    inputAngles += mouseMoveSpeed * Time.deltaTime * deltaPosition;
                    inputAngles = new Vector3(inputAngles.x, ClampAngle(inputAngles.y, verticalAngleClamp.x, verticalAngleClamp.y), 0);
                }

                if (Input.GetMouseButton(0))
                {
                    standardDirSqrMagnitude = 0;
                }
                else if (Input.GetMouseButton(1))
                {
                    standardDirSqrMagnitude = standardDirSqrMagnitudeLimit;
                }

                Quaternion quaternion = Quaternion.identity;
                Quaternion inputHorizontalRotation = Quaternion.AngleAxis(inputAngles.x, Vector3.up);
                Quaternion inputVerticalRotation = Quaternion.AngleAxis(inputAngles.y, Vector3.right);
                quaternion *= Quaternion.Slerp(inputHorizontalRotation, GetStandardHorizontalRotation(), standardDirSqrMagnitude / standardDirSqrMagnitudeLimit);
                quaternion *= inputVerticalRotation;

                trans.position = target.position + quaternion * -Vector3.forward * distance;
                trans.LookAt(target.position);
            }
        }
    }
}

