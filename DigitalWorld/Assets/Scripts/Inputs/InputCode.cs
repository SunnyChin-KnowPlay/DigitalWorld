using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Inputs
{
    /// <summary>
    /// 输入代码枚举
    /// </summary>
    public enum EventCode
    {
        #region 系统总控
        /// <summary>
        /// 退出
        /// </summary>
        Escape = 0,
        #endregion 系统总控

        #region 锁定目标
        /// <summary>
        /// 自动切换目标
        /// </summary>
        SwitchTargetAuto = 10,
        /// <summary>
        /// 切换至下一个目标
        /// </summary>
        SwitchTargetNext,
        /// <summary>
        /// 切换至上一个目标
        /// </summary>
        SwitchTargetPrev,
        #endregion 锁定目标

        #region 移动控制
        /// <summary>
        /// 前进
        /// </summary>
        MoveForward = 40,
        /// <summary>
        /// 后退
        /// </summary>
        MoveBackward,
        /// <summary>
        /// 向左平移
        /// </summary>
        MoveLeft,
        /// <summary>
        /// 向右平移
        /// </summary>
        MoveRight,
        #endregion 移动控制

        #region 快捷键
        ShortcutGroup1_0 = 100,
        ShortcutGroup1_1,
        ShortcutGroup1_2,
        ShortcutGroup1_3,
        ShortcutGroup1_4,
        ShortcutGroup1_5,
        ShortcutGroup1_6,
        ShortcutGroup1_7,
        ShortcutGroup1_8,
        ShortcutGroup1_9,

        ShortcutGroup2_0,
        ShortcutGroup2_1,
        ShortcutGroup2_2,
        ShortcutGroup2_3,
        ShortcutGroup2_4,
        ShortcutGroup2_5,
        ShortcutGroup2_6,
        ShortcutGroup2_7,
        ShortcutGroup2_8,
        ShortcutGroup2_9,
        #endregion 快捷键


    }
}
