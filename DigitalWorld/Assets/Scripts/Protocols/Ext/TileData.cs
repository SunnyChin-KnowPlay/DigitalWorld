using DigitalWorld.Table;

namespace DigitalWorld.Proto.Game
{
    public partial class TileData
    {
        public TileInfo TileInfo
        {
            get
            {
                return TableManager.instance.TileTable[this.tileId];
            }
        }

        public TilebaseInfo TilebaseInfo
        {
            get
            {
                return TableManager.instance.TilebaseTable[this.tileBaseId];
            }
        }
    }
}
