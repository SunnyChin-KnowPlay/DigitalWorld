using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    internal class Grid
    {
        public int Index => index;
        protected int index;

        public Map Map
        {
            get => map;
            set => map = value;
        }
        protected Map map;
    }
}
