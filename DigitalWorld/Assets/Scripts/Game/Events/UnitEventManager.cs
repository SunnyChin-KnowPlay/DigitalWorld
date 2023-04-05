using DreamEngine.Core;
using System.Collections.Generic;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 单位事件管理器
    /// </summary>
    public class UnitEventManager : Singleton<UnitEventManager>
    {
        #region Params
        /// <summary>
        /// 单位事件代理
        /// </summary>
        /// <param name="unit">单位</param>
        /// <param name="args"></param>
        public delegate void OnUnitHandle(UnitHandle unit, System.EventArgs args);

        /// <summary>
        /// 事件词典
        /// </summary>
        private readonly Dictionary<EUnitEventType, List<OnUnitHandle>> events = new Dictionary<EUnitEventType, List<OnUnitHandle>>();
        #endregion

        #region Logic
        public void RegisterListener(EUnitEventType type, OnUnitHandle handle)
        {
            List<OnUnitHandle> list = GetHandles(type);
            list.Add(handle);
        }

        public void UnregisterListener(EUnitEventType type, OnUnitHandle handle)
        {
            List<OnUnitHandle> list = GetHandles(type);
            list.Remove(handle);
        }

        public void Invoke(UnitHandle unit, EUnitEventType type, System.EventArgs args)
        {
            List<OnUnitHandle> list = GetHandles(type);
            if (null != list && list.Count > 0)
            {
                foreach (OnUnitHandle handle in list)
                {
                    handle.Invoke(unit, args);
                }
            }
        }

        public List<OnUnitHandle> GetHandles(EUnitEventType type)
        {
            _ = this.events.TryGetValue(type, out List<OnUnitHandle> list);
            if (null == list)
            {
                list = new List<OnUnitHandle>();
                this.events.TryAdd(type, list);
            }
            return list;
        }
        #endregion
    }
}
