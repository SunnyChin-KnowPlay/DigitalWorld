using DigitalWorld.Proto.Game;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 阻挡块
    /// </summary>
    [TileAttibute(ETileType.Block)]
    public class TileBlock : ControlTile
    {
        public override ETileType TileType => ETileType.Block;
    }
}
