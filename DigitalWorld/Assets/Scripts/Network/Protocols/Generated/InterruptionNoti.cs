using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 链接异常断开
    /// </summary>
    [ProtocolID(0xFF02)]
    public partial class InterruptionNoti : Protocol
    {
        public InterruptionNoti()
        {
            this._id = 0xFF02;
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
            return ObjectPool<InterruptionNoti>.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<InterruptionNoti>.Recycle(this);
        }

        public static InterruptionNoti Alloc()
        {
            return ObjectPool<InterruptionNoti>.Allocate();
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
