using System;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 逻辑控制器
    /// </summary>
    [Serializable]
    public abstract class ControlLogic : MonoBehaviour
    {
        #region Params
        protected UnitData data;
        protected Transform trans = null;

        public UnitControl Unit
        {
            get { return unit; }
        }
        private UnitControl unit = null;

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

        #region Mono
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

        protected virtual void OnDestroy()
        {

        }
        #endregion

        #region Setup
        public virtual void Setup(UnitControl unit, UnitData data)
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
