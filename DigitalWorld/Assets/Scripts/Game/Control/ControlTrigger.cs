using DigitalWorld.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 触发器控制器
    /// </summary>
    public class ControlTrigger : ControlLogic
    {
        #region Params
        /// <summary>
        /// 运行中的行为队列
        /// </summary>
        public List<Logic.Trigger> RunningTriggers => runningTriggers;
        protected List<Logic.Trigger> runningTriggers = new List<Logic.Trigger>();
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            this.runningTriggers.Clear();
        }

        protected override void OnDestroy()
        {
            ClearRunningTriggers();

            base.OnDestroy();
        }

        protected override void Update()
        {
            base.Update();

            int delta = Dream.FixMath.Utility.GetMillisecond(Time.deltaTime);
            for (int i = 0; i < runningTriggers.Count; ++i)
            {
                Logic.Trigger behaviour = runningTriggers[i];
                if (null != behaviour && behaviour.Enabled)
                {
                    behaviour.Update(delta);
                }
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            //检查运行中的行为队列 是否有行为已经结束了 如果是的话 直接移除
            for (int i = 0; i < runningTriggers.Count; ++i)
            {
                Logic.Trigger behaviour = runningTriggers[i];
                if (behaviour.IsEnded)
                {
                    behaviour.Recycle();
                    this.runningTriggers.RemoveAt(i);
                    --i;
                }
            }
        }
        #endregion

        #region Logic
        public virtual void RunTrigger(Logic.Trigger behaviour)
        {
            this.runningTriggers.Add(behaviour);
        }

        protected virtual void ClearRunningTriggers()
        {
            for (int i = 0; i < runningTriggers.Count; ++i)
            {
                Logic.Trigger behaviour = runningTriggers[i];
                behaviour.Recycle();
            }
            this.runningTriggers.Clear();
        }
        #endregion
    }
}
