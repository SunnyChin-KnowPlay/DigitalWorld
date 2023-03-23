using Dream.Core;
using System;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 关卡
    /// </summary>
    [Serializable]
    public partial class Level : NodeState
    {
        #region Params
        public override ENodeType NodeType => ENodeType.Level;

        public override int Id => 0;

        /// <summary>
        /// 文件的路径
        /// </summary>
        public string Path => path;
        protected string path = string.Empty;

       
        #endregion

        #region Pool
        public override object Clone()
        {
            Level level = ObjectPool<Level>.Instance.Allocate();
            this.CloneTo(level);
            return level;
        }
        #endregion

        #region Logic
        /// <summary>
        /// 唤醒
        /// </summary>
        /// <param name="ev"></param>
        public virtual void Invoke(Events.Event ev)
        {
            foreach (NodeBase node in _children)
            {
                if (node.Enabled && node is Trigger trigger)
                {
                    trigger.Invoke(ev);
                }
            }
        }


        #endregion

        
    }
}
