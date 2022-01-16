using DigitalWorld.Proto.Game;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 赌场
    /// </summary>
    [TileAttibute(ETileType.Shop)]
    public class TileShop : ControlTile
    {
        public override ETileType TileType => ETileType.Shop;
    }
}
