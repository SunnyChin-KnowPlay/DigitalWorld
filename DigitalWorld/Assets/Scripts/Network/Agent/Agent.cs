using DigitalWorld.Proto.Common;
using Dream.Core;
using Dream.Network;
using System.Net.Sockets;

namespace DigitalWorld.Net
{
    public partial class Agent : AgentBase
    {
        private string _key = null;

        public string key
        {
            get { return _key; }
            set
            {
                _key = value;
            }
        }

        public override void Recycle()
        {
            ObjectPool<Agent>.Recycle(this);
        }


        protected override void OnBreakup(object sender, SocketAsyncEventArgs e)
        {
            BreakUpNoti noti = BreakUpNoti.Alloc();
            this.PushProtocol(noti);
        }

        protected override void OnInterruption(object sender, SocketAsyncEventArgs e)
        {
            InterruptionNoti noti = InterruptionNoti.Alloc();
            this.PushProtocol(noti);
        }
    }
}
