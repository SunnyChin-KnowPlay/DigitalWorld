using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalWorld.Game
{
    public partial class ControlCharacter : ControlUnit
    {
        #region Params
        
        protected int currentMoveType;
        private const int moveTypeMask = 0x1;
        public float moveSpeed = 1f;
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();

          
        }

        protected override void Update()
        {
            base.Update();
          
        }
        #endregion

      
        #region Logic
        public override void OnBorn()
        {

        }

        public override void OnDead()
        {

        }
        #endregion
    }
}
