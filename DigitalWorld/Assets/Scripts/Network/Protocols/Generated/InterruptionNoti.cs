using Dream.Core;

namespace Dream.Network
{
    /// <summary>
    /// 链接异常断开
    /// </summary>
    [ProtocolID(0xFF02)]
    public partial class InterruptionNoti : Protocol
    {
        protected override int validByteSize => 0;

        public override ushort id => 0xFF02;

        public InterruptionNoti()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

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

        protected override void CalculateValids()
        {
            base.CalculateValids();

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
