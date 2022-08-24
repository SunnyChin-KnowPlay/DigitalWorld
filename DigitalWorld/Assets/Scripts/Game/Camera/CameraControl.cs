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
        public float distanceUp = 10f;//�����Ŀ�����ֱ�߶Ȳ���
        public float distanceAway = 10f;//�����Ŀ���ˮƽ�������
        public float smooth = 2f;//λ��ƽ���ƶ���ֵ����ֵ
        public float camDepthSmooth = 20f;

        private Transform trans;

        private void Start()
        {
            trans = this.transform;
        }

        void Update()
        {
            // �������������Զ��
            if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) || Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80)
            {
                Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
            }
        }

        void LateUpdate()
        {
            if (null != target)
            {
                //����������λ��
                Vector3 disPos = target.position + Vector3.up * distanceUp - target.forward * distanceAway;

                trans.position = Vector3.Lerp(trans.position, disPos, Time.deltaTime * smooth);
                //����ĽǶ�
                trans.LookAt(target.position);
            }


        }
    }

}

