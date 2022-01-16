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
        /// 等级
        /// </summary>
        Level = 3,
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

        /// <summary>
        /// 地块
        /// </summary>
        Tile = 2,
    }
    /// <summary>
    /// 地块类型
    /// </summary>
    public enum ETileType : int
    {

        /// <summary>
        /// 未定义
        /// </summary>
        None = 0,

        /// <summary>
        /// 出生点
        /// </summary>
        Origin = 1,

        /// <summary>
        /// 赌场
        /// </summary>
        Casino = 2,

        /// <summary>
        /// 宝箱
        /// </summary>
        Chest = 3,

        /// <summary>
        /// 门
        /// </summary>
        Door = 4,

        /// <summary>
        /// 
        /// </summary>
        Grass = 5,

        /// <summary>
        /// 魔晶
        /// </summary>
        MagicStone = 6,

        /// <summary>
        /// 怪物
        /// </summary>
        Monster = 7,

        /// <summary>
        /// 山
        /// </summary>
        Mountion = 8,

        /// <summary>
        /// 商店
        /// </summary>
        Shop = 9,

        /// <summary>
        /// 阻挡
        /// </summary>
        Block = 10,

        /// <summary>
        /// 旅行者
        /// </summary>
        Traveller = 11,
    }
    /// <summary>
    /// 地块数据
    /// </summary>
    [ProtocolID(0x0100)]
    public partial class TileData : Protocol
    {
        public const ushort protocolId = 0x0100;

        public override ushort Id => protocolId;

        private int _tileId;
        /// <summary>
        /// 配置ID
        /// </summary>
        public int tileId { get { return _tileId; } set { _tileId = value; } }
        private int _index;
        /// <summary>
        /// 索引号
        /// </summary>
        public int index { get { return _index; } set { _index = value; } }
        private int _tileBaseId;
        /// <summary>
        /// 地基ID
        /// </summary>
        public int tileBaseId { get { return _tileBaseId; } set { _tileBaseId = value; } }
        private int _objectId;
        /// <summary>
        /// 物件ID
        /// </summary>
        public int objectId { get { return _objectId; } set { _objectId = value; } }
        private int _level;
        /// <summary>
        /// 等级
        /// </summary>
        public int level { get { return _level; } set { _level = value; } }
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
            _index = default(int);
            _tileBaseId = default(int);
            _objectId = default(int);
            _level = default(int);
        }

        public override Protocol Allocate()
        {
            return ObjectPool<TileData>.Instance.Allocate();
        }

        public static TileData Alloc()
        {
            return ObjectPool<TileData>.Instance.Allocate();
        }

        #region Encode
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._tileId);
            this.Encode(this._index);
            this.Encode(this._tileBaseId);
            this.Encode(this._objectId);
            this.Encode(this._level);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._tileId, "tileId");
            this.Encode(this._index, "index");
            this.Encode(this._tileBaseId, "tileBaseId");
            this.Encode(this._objectId, "objectId");
            this.Encode(this._level, "level");
        }
        #endregion

        #region Decode
        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._tileId);
            this.Decode(ref this._index);
            this.Decode(ref this._tileBaseId);
            this.Decode(ref this._objectId);
            this.Decode(ref this._level);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._tileId, "tileId");
            this.Decode(ref this._index, "index");
            this.Decode(ref this._tileBaseId, "tileBaseId");
            this.Decode(ref this._objectId, "objectId");
            this.Decode(ref this._level, "level");
        }
        #endregion

        #region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._tileId);
            this.CalculateSize(this._index);
            this.CalculateSize(this._tileBaseId);
            this.CalculateSize(this._objectId);
            this.CalculateSize(this._level);
        }
        #endregion
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
        private List<TileData> _tiles;
        /// <summary>
        /// 地块队列
        /// </summary>
        public List<TileData> tiles { get { return _tiles; } set { _tiles = value; } }
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
            _tiles = default(List<TileData>);
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
        protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);

            this.Encode(this._mapId);
            this.Encode(this._level);
            this.Encode(this._tiles);
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            this.Encode(this._mapId, "mapId");
            this.Encode(this._level, "level");
            this.Encode(this._tiles, "tiles");
        }
        #endregion

        #region Decode
        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);

            this.Decode(ref this._mapId);
            this.Decode(ref this._level);
            this.Decode(ref this._tiles);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            this.Decode(ref this._mapId, "mapId");
            this.Decode(ref this._level, "level");
            this.Decode(ref this._tiles, "tiles");
        }
        #endregion

        #region Calculate Size
        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            this.CalculateSize(this._mapId);
            this.CalculateSize(this._level);
            this.CalculateSize(this._tiles);
        }
        #endregion
    }

}