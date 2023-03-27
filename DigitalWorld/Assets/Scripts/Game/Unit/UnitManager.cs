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

        public abstract EUnitType Type { get; }

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

            ControlUnit unit = handle.Unit;
            unit.transform.SetParent(transform, false);
        }

        public virtual void UnregisterUnit(UnitHandle handle)
        {
            ControlUnit unit = handle.Unit;
            unit.transform.SetParent(null, false);

            if (this.units.ContainsKey(handle.Uid))
            {
                this.units.Remove(handle.Uid);
            }
        }
        #endregion
    }
}
