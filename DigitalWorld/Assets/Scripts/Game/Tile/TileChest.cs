using DigitalWorld.Proto.Game;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 赌场
    /// </summary>
    [TileAttibute(ETileType.Chest)]
    public class TileChest : ControlTile
    {
        public override ETileType TileType => ETileType.Chest;
    }
}
