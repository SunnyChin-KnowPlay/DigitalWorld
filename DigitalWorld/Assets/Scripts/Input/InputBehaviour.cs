using DigitalWorld.Game;
using UnityEngine;

namespace DigitalWorld.Behaviour
{
    public class InputBehaviour : MonoBehaviour
    {
        /// <summary>
        /// µ¥Î»¿ØÖÆÆ÷
        /// </summary>
        private ControlCharacter unit;

        private void Awake()
        {
            unit = this.GetComponent<ControlCharacter>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (null != unit)
            {
                Vector3 movingDir = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    movingDir += Vector3.forward;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    movingDir += Vector3.left;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    movingDir += Vector3.back;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    movingDir += Vector3.right;
                }

                unit.Move.ApplyMove(movingDir);
            }
        }
    }

}

