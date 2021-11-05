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

        public override void OnAllocate()
        {
            base.OnAllocate();
            this._isSyncProcess = true;
        }

        protected override void OnBreakup(object sender, SocketAsyncEventArgs e)
        {
            NotiBreakUp noti = NotiBreakUp.Alloc();
            this.PushProtocol(noti);
        }

        protected override void OnInterruption(object sender, SocketAsyncEventArgs e)
        {
            NotiInterruption noti = NotiInterruption.Alloc();
            this.PushProtocol(noti);
        }
    }
}
