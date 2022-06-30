using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic
{
    public partial class Level : NodeFSE
    {
        #region Params
        /// <summary>
        /// 配置模板触发器队列
        /// </summary>
        private List<Trigger> triggers = new List<Trigger>();

        /// <summary>
        /// 运行触发器队列
        /// </summary>
        private List<Trigger> runningTriggers = new List<Trigger>();
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
            triggers.Clear();
            runningTriggers.Clear();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            for (int i = 0; i < triggers.Count; ++i)
            {
                Trigger trigger = triggers[i];
                trigger.Recycle();
            }
            this.triggers.Clear();

            for (int i = 0; i < runningTriggers.Count; ++i)
            {
                Trigger trigger = runningTriggers[i];
                trigger.Recycle();
            }
            this.runningTriggers.Clear();
        }
        #endregion

        #region Logic
        protected override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);

            UpdateRunningTriggers(delta);
        }

        private void UpdateRunningTriggers(float delta)
        {
            for (int i = 0; i < runningTriggers.Count; ++i)
            {
                Trigger trigger = runningTriggers[i];
                if (null != trigger)
                {
                    trigger.Update(delta);

                    if (trigger.State == EState.Idle)
                    {
                        trigger.Recycle();
                        runningTriggers.RemoveAt(i);
                        --i;
                    }
                }
            }
        }

        public void DispatchEvent(Event ev)
        {
            Trigger trigger = null;
            for (int i = 0; i < triggers.Count; i++)
            {
                trigger = triggers[i];

                if (!trigger.Enabled) continue;
                if (trigger.ListenEventId == ev.Id)
                {

                }
            }
        }
        #endregion
    }
}
