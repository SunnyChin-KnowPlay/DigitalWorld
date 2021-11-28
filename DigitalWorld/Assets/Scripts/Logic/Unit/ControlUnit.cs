using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Logic
{
    public enum EMoveType
    {
        None = 0,
        Forward = 1 << 0,
        Right = 1 << 1,
        Back = 1 << 2,
        Left = 1 << 3,
    }


    public class ControlUnit : MonoBehaviour
    {
        #region Params
        protected NavMeshAgent navMeshAgent;
        protected int currentMoveType;
        private const int moveTypeMask = 0x1;

        public float moveSpeed = 1f;
        #endregion



        #region Behaviour
        protected virtual void Awake()
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            UpdateMove();
        }
        #endregion

        #region Move
        public void AddMoveDir(EMoveType t)
        {
            this.currentMoveType |= moveTypeMask << (int)t;
        }

        public void RemoveMoveDir(EMoveType t)
        {
            this.currentMoveType &= ~(moveTypeMask << (int)t);
        }

        private bool CheckMoveType(EMoveType t)
        {
            return ((this.currentMoveType >> (int)t) & moveTypeMask) == moveTypeMask;
        }

        private void UpdateMove()
        {
            Vector3 v = Vector3.zero;

            if (this.CheckMoveType(EMoveType.Forward))
            {
                v += Vector3.forward;
            }
            if (this.CheckMoveType(EMoveType.Right))
            {
                v += Vector3.right;
            }
            if (this.CheckMoveType(EMoveType.Back))
            {
                v += Vector3.back;
            }
            if (this.CheckMoveType(EMoveType.Left))
            {
                v += Vector3.left;
            }

            v.Normalize();
            v = v * Time.deltaTime * moveSpeed;

            this.Move(v);
        }

        public virtual void Move(Vector3 offset)
        {
            if (null != navMeshAgent)
            {
                navMeshAgent.Move(offset);
            }
        }
        #endregion
    }

}

