using Dream.Network;
using System.Net.Sockets;

namespace DigitalWorld.Network
{
    public partial class Agent : AgentBase
    {
        public override void Recycle()
        {
            throw new System.NotImplementedException();
        }

        protected override void AllocateProtocols()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnBreakup(object sender, SocketAsyncEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInterruption(object sender, SocketAsyncEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
