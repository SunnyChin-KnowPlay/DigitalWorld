using DigitalWorld.Proto.Common;
using Dream.Core;
using Dream.Network;
using Dream.Proto;

namespace DigitalWorld.Net
{
    public partial class Agent : AgentBase
    {

        protected override Protocol AllocateProtocol(ushort protocolID)
        {
            EnumProtocolID id = (EnumProtocolID)protocolID;
            switch (id)
            {
                case EnumProtocolID.ReqLogin:
                    return ObjectPool<ReqLogin>.Instance.Allocate();
                case EnumProtocolID.AckLogin:
                    return ObjectPool<AckLogin>.Instance.Allocate();
                case EnumProtocolID.NotiError:
                    return ObjectPool<NotiError>.Instance.Allocate();
            }
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
