using DigitalWorld.Proto.Game;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 赌场
    /// </summary>
    [TileAttibute(ETileType.Traveller)]
    public class TileTraveller : ControlTile
    {
        public override ETileType TileType => ETileType.Traveller;
    }
}
