using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 属性基类
    /// </summary>
    public abstract partial class PropertyBase : NodeBase
    {
        #region Params
        public override ENodeType NodeType => ENodeType.Property;

        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
        }
        #endregion

    }
}
