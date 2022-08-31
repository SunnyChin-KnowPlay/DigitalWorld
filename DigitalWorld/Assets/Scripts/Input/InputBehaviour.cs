using DigitalWorld.Game;
using UnityEngine;

namespace DigitalWorld.Behaviour
{
    public class InputBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 单位控制器
        /// </summary>
        private ControlCharacter unit;
        private Transform trans;

        public const float mouseDirMoveSpeed = 90;
        public const float keyboardDirMoveSpeed = 20;


        private Vector3 oldPosition;

        private void Awake()
        {
            unit = this.GetComponent<ControlCharacter>();
            trans = this.transform;
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            if (null != unit)
            {
                Vector3 movingDir = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    movingDir += Vector3.forward;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    movingDir -= Vector3.forward;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    movingDir -= Vector3.right;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    movingDir += Vector3.right;
                }

                unit.Move.ApplyMove(movingDir);

                UpdateDir();
            }
        }

        private void UpdateDir()
        {
            if (Input.GetMouseButtonDown(1))
            {
                this.oldPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 deltaPostion = Input.mousePosition - this.oldPosition;
                this.oldPosition = Input.mousePosition;

                //获取鼠标旋转的度数 纵轴
                float rotationAmount = deltaPostion.x * mouseDirMoveSpeed * Time.deltaTime;
                trans.Rotate(Vector3.up, rotationAmount);
            }
        }
    }

}

