using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 单位管理器
    /// </summary>
    public abstract class UnitManager : MonoBehaviour
    {
        #region Params
        public abstract string Name { get; }
        /// <summary>
        /// 该管理器的单位类型
        /// </summary>
        public abstract EUnitType Type { get; }

        /// <summary>
        /// 单位词典
        /// </summary>
        protected readonly Dictionary<uint, UnitHandle> units = new Dictionary<uint, UnitHandle>();
        #endregion

        #region Register & Unregister
        public virtual void RegisterUnit(UnitHandle handle)
        {
            if (this.units.ContainsKey(handle.Uid))
            {
                this.units[handle.Uid] = handle;
            }
            else
            {
                this.units.Add(handle.Uid, handle);
            }

            UnitControl unit = handle.Unit;
            unit.transform.SetParent(transform, false);
        }

        public virtual void UnregisterUnit(UnitHandle handle)
        {
            UnitControl unit = handle.Unit;
            unit.transform.SetParent(null, false);

            if (this.units.ContainsKey(handle.Uid))
            {
                this.units.Remove(handle.Uid);
            }
        }
        #endregion

        #region Mono
        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void OnDestroy()
        {

        }
        #endregion
    }
}
