using System.Xml;

namespace DigitalWorld.Logic.Properties
{
    /// <summary>
    /// 属性基类
    /// </summary>
    public abstract partial class PropertyBase : NodeBase, System.ICloneable
    {
        #region Params

        /// <summary>
        /// 节点类型
        /// </summary>
        public override ENodeType NodeType => ENodeType.Property;


        #endregion

        #region Clone

        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is PropertyBase v)
            {

            }

            return obj;
        }
        #endregion

        #region Logic
        public abstract object GetValue();
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
        }
        #endregion

        
    }
}
