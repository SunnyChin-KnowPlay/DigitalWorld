using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeAction : NodeItem
    {
        #region Common
        public override string GetTitle()
        {
            return "行动";
        }

        public override object Clone()
        {
            NodeAction v = new NodeAction();
            this.CloneTo(v);
            return v;
        }
        #endregion
    }
}
