using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// ս�����������
    /// </summary>
    public class CameraControl : MonoBehaviour
    {
        public Transform target;
        public float distance = 15;

        public Vector2 distanceClamp = new Vector2(5, 25);
        /// <summary>
        /// ������ӽ� ��Ŀ������������ĸ߶ȽǶ�
        /// </summary>
        public float heightAngleOfView = 45;
        public Vector2 heightAngleClamp = new Vector2(15, 75);
        public const float mouseScrollWheelSpeed = 1;

        /// <summary>
        /// �����Ŀ���ƫ����
        /// </summary>
        private Vector3 offset;
        private Vector3 oldMousePosition;
        public const float mouseHeightAngleMoveSpeed = 90;
        /// <summary>
        /// �Ƿ�ס�����
        /// </summary>
        private bool isPressingLeftMouse = false;

        /// <summary>
        /// �������תŷ����
        /// </summary>
        private Vector3 leftMouseEulers;

        protected Vector3 lookAtRotation;
        public float mouseTurnedSpeed;

        protected Transform trans;


        private void Start()
        {
            trans = this.transform;
            leftMouseEulers = Vector3.zero;
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
                leftMouseEulers = target.rotation.eulerAngles;
                isPressingLeftMouse = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isPressingLeftMouse = false;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                leftMouseEulers += new Vector3(deltaPosition.y, deltaPosition.x, 0);

            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                this.oldMousePosition = Input.mousePosition;

                this.heightAngleOfView = Mathf.Clamp(this.heightAngleOfView + deltaPosition.y * mouseHeightAngleMoveSpeed * Time.deltaTime, this.heightAngleClamp.x, this.heightAngleClamp.y);
                UnityEngine.Debug.Log(heightAngleOfView);
            }

            if (null != target)
            {
                offset = Vector3.zero;

                if (isPressingLeftMouse) // �������������� ����ֱ���ƶ���ͷ�� 
                {
                    
                }
                else
                {
                    offset += distance * Mathf.Sin(heightAngleOfView * Mathf.Deg2Rad) * target.up;
                    offset += distance * Mathf.Cos(heightAngleOfView * Mathf.Deg2Rad) * -target.forward;
                }

                trans.position = target.position + offset;
                trans.LookAt(target.position);
            }
        }


    }

}

