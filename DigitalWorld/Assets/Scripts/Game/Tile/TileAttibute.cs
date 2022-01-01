using System;

namespace DigitalWorld.Game
{
    public class TileAttibute : Attribute
    {
        private ETileType type;

        public ETileType Type
        {
            get { return type; }
            set { type = value; }
        }

        public TileAttibute(ETileType type)
        {
            this.type = type;
        }
    }
}
