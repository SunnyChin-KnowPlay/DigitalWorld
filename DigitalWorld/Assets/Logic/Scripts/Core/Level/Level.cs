using DigitalWorld.Asset;
using DigitalWorld.Logic;
using Dream.Core;

using System.Xml;
using UnityEngine;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 关卡
    /// </summary>
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

        #region Load
       

       

       

        #endregion

        #region Proto
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();


        }

        protected override void OnEncode()
        {
            base.OnEncode();


        }

        protected override void OnDecode()
        {
            base.OnDecode();


        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);


        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);


        }
        #endregion
    }
}
