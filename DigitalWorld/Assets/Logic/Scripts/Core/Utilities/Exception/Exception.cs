namespace DigitalWorld.Logic
{
    internal class NodeException : System.Exception
    {
        #region Params
        protected NodeBase node;
        #endregion

        internal NodeException(NodeBase node, string message)
            : base(message)
        {
            this.node = node;
        }

        public override string Message
        {
            get
            {
                return string.Format("Logic Exception. [Node name is:{0}], [message is:{1}]", this.node.Name, base.Message);
            }
        }
    }
}
