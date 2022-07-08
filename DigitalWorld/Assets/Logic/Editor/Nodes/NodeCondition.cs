using System.Xml;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeCondition : NodeItem
    {

       
        #region Common
        public override string GetTitle()
        {
            return "条件";
        }

        public override object Clone()
        {
            NodeCondition v = new NodeCondition();
            this.CloneTo(v);
            return v;
        }
        #endregion
    }
}
