using DigitalWorld.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalWorld.Inputs
{
    public class InputBehaviour : MonoBehaviour
    {
        /// <summary>
        /// ��λ������
        /// </summary>
        private ControlCharacter unit;
        private Transform trans;

        public const float mouseDirMoveSpeed = 1;
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
                do
                {
                    UpdateMove();
                    UpdateDir();

                } while (false);

            }
        }

        private void UpdateFunctions()
        {

        }

        private void UpdateMove()
        {
            Vector3 movingDir = Vector3.zero;
            if (Input.GetKey(InputManager.Instance.GetKeyCode(EventCode.MoveForward)))
            {
                movingDir += Vector3.forward;
            }

            if (Input.GetKey(InputManager.Instance.GetKeyCode(EventCode.MoveBackward)))
            {
                movingDir -= Vector3.forward;
            }

            if (Input.GetKey(InputManager.Instance.GetKeyCode(EventCode.MoveLeft)))
            {
                movingDir -= Vector3.right;
            }

            if (Input.GetKey(InputManager.Instance.GetKeyCode(EventCode.MoveRight)))
            {
                movingDir += Vector3.right;
            }

            unit.Move.ApplyMove(movingDir);
        }

        private void UpdateDir()
        {
            if (!IsUITouching)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    this.oldPosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(1))
                {
                    Vector3 deltaPostion = Input.mousePosition - this.oldPosition;
                    this.oldPosition = Input.mousePosition;

                    //��ȡ�����ת�Ķ��� ����
                    float rotationAmount = deltaPostion.x * mouseDirMoveSpeed;
                    trans.Rotate(Vector3.up, rotationAmount);
                }
            }
        }

        /// <summary>
        /// ��ǰUI�Ƿ����ڴ�����
        /// </summary>
        /// <returns></returns>
        private static bool IsUITouching
        {
            get
            {
                return EventSystem.current.IsPointerOverGameObject() || GUIUtility.hotControl != 0;
            }
        }

    }

}

