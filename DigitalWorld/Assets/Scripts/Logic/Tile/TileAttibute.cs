using System;

namespace DigitalWorld.Logic
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
