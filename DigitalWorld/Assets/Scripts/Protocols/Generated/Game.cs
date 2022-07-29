using Dream.Core;
using Dream.Proto;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Proto.Game
{
        	/// <summary>
    /// 属性类型
    /// </summary>
    public enum EPropertyType : int
    {
  
        /// <summary>
        /// 血量
        /// </summary>
        Hp = 1,
  
        /// <summary>
        /// 攻击力
        /// </summary>
        Attack = 2,
  
        /// <summary>
        /// 移动速度
        /// </summary>
        MoveSpeed = 3,
    }
        	/// <summary>
    /// 单位类型
    /// </summary>
    public enum EUnitType : int
    {
  
        /// <summary>
        /// 角色
        /// </summary>
        Character = 1,
    }
            /// <summary>
    /// 地图数据
    /// </summary>
    [ProtocolID(0x0101)]
    public partial class MapData : Protocol
    {
        public const ushort protocolId = 0x0101;

        public override ushort Id => protocolId;

        private int _mapId;
        /// <summary>
        /// 地图ID
        /// </summary>
        public int mapId { get { return _mapId; } set { _mapId = value; } }
        private int _level;
        /// <summary>
        /// 等级
        /// </summary>
        public int level { get { return _level; } set { _level = value; } }
        public MapData()
        {
        }

        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _mapId = default(int);
            _level = default(int);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<MapData>.Instance.Allocate();
        }

        public static MapData Alloc()
        {
            return ObjectPool<MapData>.Instance.Allocate();
        }

#region Encode
        protected override void OnEncode()
        {
            base.OnEncode();

            this.Encode(this._mapId);
            this.Encode(this._level);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._mapId, "mapId");
            this.Encode(this._level, "level");
        }
#endregion

#region Decode
        protected override void OnDecode()
        {
            base.OnDecode();

            this.Decode(ref this._mapId);
            this.Decode(ref this._level);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._mapId, "mapId");
            this.Decode(ref this._level, "level");
        }
#endregion

#region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._mapId);
            this.CalculateSize(this._level);
        }
#endregion
    }

}