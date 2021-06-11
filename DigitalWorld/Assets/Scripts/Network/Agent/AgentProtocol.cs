using Dream.Core;
using Dream.Network;

namespace DigitalWorld.Net
{
    public partial class Agent : AgentBase
    {
        protected override void AllocateProtocols()
        {
            this.PreallocateProtocol(ObjectPool<LoginReq>.Allocate());
            this.PreallocateProtocol(ObjectPool<LoginAck>.Allocate());
            this.PreallocateProtocol(ObjectPool<ErrorNoti>.Allocate());
            this.PreallocateProtocol(ObjectPool<BreakUpNoti>.Allocate());
            this.PreallocateProtocol(ObjectPool<InterruptionNoti>.Allocate());

            this.RegisterListeners();
        }

        private void RegisterListeners()
        {
            this.SetProtocolListener<BreakUpNoti>(OnProcessBreakUpNoti);
        }

        private void OnProcessBreakUpNoti(Protocol p)
        {

        }
    }

}
