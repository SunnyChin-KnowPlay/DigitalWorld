using DigitalWorld.Table;

namespace DigitalWorld.Proto.Game
{
    public partial class TileData
    {
        public TilebaseInfo TilebaseInfo
        {
            get
            {
                return TableManager.instance.TilebaseTable[this.tileBaseId];
            }
        }
    }
}
