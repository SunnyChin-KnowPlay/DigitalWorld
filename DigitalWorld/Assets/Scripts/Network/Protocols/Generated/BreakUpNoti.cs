using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 正常分手断链
    /// </summary>
    [ProtocolID(0xFF01)]
    public partial class BreakUpNoti : Protocol
    {
        public BreakUpNoti()
        {
            this._id = 0xFF01;
        }

        public override void OnAllocate()
        {
            base.OnAllocate();

        }

        public override void OnRecycle()
        {

        }

        public override Protocol Allocate()
        {
            return ObjectPool<BreakUpNoti>.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<BreakUpNoti>.Recycle(this);
        }

        public static BreakUpNoti Alloc()
        {
            return ObjectPool<BreakUpNoti>.Allocate();
        }

        public override void Encode(byte[] buffer, int pos)
        {
            base.Encode(buffer, pos);

        }

        public override void Decode(byte[] buffer, int pos)
        {
            base.Decode(buffer, pos);

        }
    }
}
