﻿using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 逻辑控制器
    /// </summary>
    public abstract class ControlLogic : MonoBehaviour
    {
        #region Params
        protected UnitData data;
        protected Transform trans = null;

        public ControlUnit Unit
        {
            get { return unit; }
        }
        private ControlUnit unit = null;

        public UnitHandle UnitHandle
        {
            get
            {
                return new UnitHandle(unit);
            }
        }

        public virtual UnitData Data
        {
            get { return data; }
        }
        #endregion

        #region Behaviour
        protected virtual void Awake()
        {
            trans = this.GetComponent<Transform>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Destroy()
        {

        }
        #endregion

        #region Setup
        public virtual void Setup(ControlUnit unit, UnitData data)
        {
            this.data = data;
            this.unit = unit;
        }
        #endregion

        #region Gizmos
        protected virtual void OnDrawGizmos()
        {
            
        }

        protected virtual void OnDrawGizmosSelected()
        {

        }
        #endregion
    }
}
