using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 行为控制器
    /// </summary>
    public class ControlBehaviour : ControlLogic
    {
        #region Params
        /// <summary>
        /// 运行中的行为队列
        /// </summary>
        public List<Logic.Behaviour> RunningBehaviours => runningBehaviours;
        protected List<Logic.Behaviour> runningBehaviours = new List<Logic.Behaviour>();
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            this.runningBehaviours.Clear();
        }

        public override void Destroy()
        {
            base.Destroy();

            ClearRunningBehaviours();
        }

        protected override void Update()
        {
            base.Update();

            float delta = Time.deltaTime;
            for (int i = 0; i < runningBehaviours.Count; ++i)
            {
                Logic.Behaviour behaviour = runningBehaviours[i];
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
            for (int i = 0; i < runningBehaviours.Count; ++i)
            {
                Logic.Behaviour behaviour = runningBehaviours[i];
                if (behaviour.State == Logic.NodeState.EState.Ended)
                {
                    behaviour.Recycle();
                    this.runningBehaviours.RemoveAt(i);
                    --i;
                }
            }
        }
        #endregion

        #region Logic
        protected void ClearRunningBehaviours()
        {
            for (int i = 0; i < runningBehaviours.Count; ++i)
            {
                Logic.Behaviour behaviour = runningBehaviours[i];
                behaviour.Recycle();
            }
            this.runningBehaviours.Clear();
        }
        #endregion
    }
}
