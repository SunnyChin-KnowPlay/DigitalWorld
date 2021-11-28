using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public class InputControl : MonoBehaviour
    {
        /// <summary>
        /// µ¥Î»¿ØÖÆÆ÷
        /// </summary>
        private ControlUnit unit;




        private void Awake()
        {
            unit = this.GetComponent<ControlUnit>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                unit.AddMoveDir(EMoveType.Forward);
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                unit.AddMoveDir(EMoveType.Right);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                unit.AddMoveDir(EMoveType.Back);
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                unit.AddMoveDir(EMoveType.Left);
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                unit.RemoveMoveDir(EMoveType.Forward);
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                unit.RemoveMoveDir(EMoveType.Right);
            }

            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                unit.RemoveMoveDir(EMoveType.Back);
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                unit.RemoveMoveDir(EMoveType.Left);
            }
        }


    }

}

