using System;
using System.Collections.Generic;

namespace DigitalWorld.Table.Editor
{
    [Serializable]
    internal class Model
    {
        public string NamespaceName { get; set; }
        public List<NodeModel> models = new List<NodeModel>();
    }
}
