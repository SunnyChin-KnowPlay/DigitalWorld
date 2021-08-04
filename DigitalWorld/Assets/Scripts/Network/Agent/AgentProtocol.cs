using Dream.Core;
using Dream.Network;
using Dream.Proto;

namespace DigitalWorld.Net
{
    public partial class Agent : AgentBase
    {

        protected override Protocol AllocateProtocol(ushort protocolID)
        {
            return null;
        }


        private void RegisterListeners()
        {
            //this.SetProtocolListener<BreakUpNoti>(OnProcessBreakUpNoti);
        }

        private void OnProcessBreakUpNoti(Protocol p)
        {

        }
    }

}
