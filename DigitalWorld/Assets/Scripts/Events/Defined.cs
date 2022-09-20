using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Events
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EEventType : int
    {
        /// <summary>
        /// Esc
        /// </summary>
        Escape = 0,
    }

    /// <summary>
    /// 句柄添加模式
    /// </summary>
    public enum EHandleAddMode
    {
        /// <summary>
        /// 直接
        /// </summary>
        Force = 0,
        /// <summary>
        /// 替换(如果存在有一样的)
        /// </summary>
        Replace,
    }

    /// <summary>
    /// 句柄移除模式
    /// </summary>
    public enum EHandleRemoveMode
    {
        /// <summary>
        /// 向前搜索 从索引0开始向count - 1方向
        /// </summary>
        Forward,
        /// <summary>
        /// 向后搜索 从索引count - 1开始向0方向
        /// </summary>
        Backward,
        /// <summary>
        /// 全部
        /// </summary>
        All,
    }

}
