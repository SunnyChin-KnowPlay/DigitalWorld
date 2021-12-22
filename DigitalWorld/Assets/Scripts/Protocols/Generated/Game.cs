using Dream.Core;
using Dream.Proto;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Proto.Game
{
    /// <summary>
    /// 地块数据
    /// </summary>
    [ProtocolID(0x0100)]
    public partial class TileData : Protocol
    {
        protected override int ValidByteSize => 1;

        public const ushort protocolId = 0x0100;

        public override ushort Id => protocolId;

        private int _tileId;
        /// <summary>
        /// 配置ID
        /// </summary>
        public int tileId { get { return _tileId; } set { _tileId = value; } }
        private int _tileBaseId;
        /// <summary>
        /// 地基ID
        /// </summary>
        public int tileBaseId { get { return _tileBaseId; } set { _tileBaseId = value; } }
        public TileData()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _tileId = default(int);
            _tileBaseId = default(int);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<TileData>.Instance.Allocate();
        }

        public static TileData Alloc()
        {
            return ObjectPool<TileData>.Instance.Allocate();
        }

        protected override void CalculateValids()
        {
            base.CalculateValids();

            this.SetParamValid(0, this._tileId != default(int));
            this.SetParamValid(1, this._tileBaseId != default(int));
        }

        #region Encode
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Encode(this._tileId);
            if (this.CheckIsParamValid(1))
                this.Encode(this._tileBaseId);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._tileId, "tileId");
            this.Encode(this._tileBaseId, "tileBaseId");
        }
        #endregion

        #region Decode
        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            if (this.CheckIsParamValid(0))
                this.Decode(ref this._tileId);
            if (this.CheckIsParamValid(1))
                this.Decode(ref this._tileBaseId);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._tileId, "tileId");
            this.Decode(ref this._tileBaseId, "tileBaseId");
        }
        #endregion

        #region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._tileId);
            this.CalculateSize(this._tileBaseId);
        }
        #endregion
    }

}