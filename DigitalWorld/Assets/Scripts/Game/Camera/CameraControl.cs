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

        private Vector3 offset;
        private bool rightButtonPressed = false;

        protected Vector3 lookAtRotation;
        public float mouseTurnedSpeed;

        protected Transform trans;

        //��¼��һframe����ת��
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
                    //��ȡ�����ת�Ķ��� ����
                    float rotationAmount = Input.GetAxis("Mouse X") * mouseTurnedSpeed * Time.deltaTime;
                    if (rotationAmount != 0)
                    {
                        int x = 1;
                    }
                    //���յ���ת����
                    trans.RotateAround(target.position, Vector3.up, rotationAmount * 360);
                    //����Ҳ��ת ��֤��ͷʼ�ն������ﱳ��
                    target.RotateAround(target.position, Vector3.up, rotationAmount * 360);

                    ////����
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

