using System.Xml;
using UnityEditor;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeEvent : NodeItem
    {
       
        #region Common
        public override string GetTitle()
        {
            return "Event";
        }

        public override object Clone()
        {
            NodeEvent v = new NodeEvent();
            this.CloneTo(v);
            return v;
        }

        #endregion

    }
}
